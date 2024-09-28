using System.Net;

namespace NugetPackage.GenericHttpClient.Services;
public sealed class ExceptionsManager : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ResponseContent { get; }

    public ExceptionsManager(HttpStatusCode statusCode, string responseContent)
        : base($"HTTP Request failed with status code {(int)statusCode} ({statusCode}).")
    {
        StatusCode = statusCode;
        ResponseContent = responseContent;
    }
}