using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using NugetPackage.MiddlewareLogs.Contracts;
using NugetPackage.MiddlewareLogs.NewFolder;
using System.Text;

namespace NugetPackage.MiddlewareLogs;

public sealed class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
    private RequestResponseData log { get; set; }

    private readonly MiddlewareConfig _middlewareConfig;
    public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IOptions<MiddlewareConfig> middlewareConfig)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        _middlewareConfig = middlewareConfig.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        //if (context.Request.Path.ToString().Contains("/api/") && context.Request.Method != "GET")
        log = new RequestResponseData()
        {
            Application = _middlewareConfig.ApplicationName,
            ApplicationVersion = _middlewareConfig.ApplicationVersion,
            User = context.User?.Identity?.Name
        };

        await LogRequest(context);
        await LogResponse(context);

        _logger.LogInformation(log.ToString());
    }

    private async Task LogRequest(HttpContext context)
    {
        try
        {
            StringBuilder headers = new();

            var requestBodyStream = new MemoryStream();

            foreach (var key in context.Request.Headers.Keys)
            {
                headers.Append(string.Concat(key, "=", context.Request.Headers[key], Environment.NewLine));
            }
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            log.RequestTimestamp = DateTime.Now;
            log.RequestUri = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            log.IpAddress = $"{context.Request.Host.Host}";
            log.RequestMethod = $"{context.Request.Method}";
            log.Machine = Environment.MachineName;
            log.RequestContentType = context.Request.ContentType;
            log.RequestBody = ReadStreamInChunks(requestStream);
            log.RequestHeaders = headers.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "request/response logging exception");
            throw;
        }
        //context.Request.Body.Position = 0;
    }
    private async Task LogResponse(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
        context.Response.Body = responseBody;

        await _next(context);

        log.ResponseTimestamp = DateTime.Now;
        log.ResponseBody = await FormatResponse(context.Response);
        log.ResponseStatusCode = context.Response.StatusCode;

        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkBufferLength = 4096;
        stream.Seek(0, SeekOrigin.Begin);
        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);
        var readChunk = new char[readChunkBufferLength];
        int readChunkLength;
        do
        {
            readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);
        } while (readChunkLength > 0);
        return textWriter.ToString();
    }

    private string FormatRequest(HttpRequest request)
    {
        var body = request.Body;

        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        request.Body.ReadAsync(buffer, 0, buffer.Length);
        var bodyAsText = Encoding.UTF8.GetString(buffer);

        return $"{request.QueryString} {bodyAsText}";
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return $"{text}";
    }
}