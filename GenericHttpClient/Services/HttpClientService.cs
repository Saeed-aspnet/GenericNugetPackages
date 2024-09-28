using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NugetPackage.GenericHttpClient.Interfaces;
using System.Text;

namespace NugetPackage.GenericHttpClient.Services;
public sealed class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpClientService> _logger;
    public HttpClientService(IHttpClientFactory httpClientFactory, ILogger<HttpClientService> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public void AddAuthorizationHeader(string token)
    {
        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }

    public void RemoveAuthorizationHeader()
    {
        if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }
    }

    public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending request to {Url}", url);
            var response = await _httpClient.GetAsync(url, cancellationToken);
            await EnsureSuccessStatusCode(response);
            _logger.LogInformation("Request to {Url} successfull", url);
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken);

            CheckResponseData(" ", url, responseData);

            return JsonConvert.DeserializeObject<TResponse>(responseData);
        }
        catch (ExceptionsManager ex)
        {
            _logger.LogError(ex, $"Request to {url} failed");
            throw;
        }
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending request to {Url}", url);
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            await EnsureSuccessStatusCode(response);
            _logger.LogInformation("Request to {Url} successfull", url);

            var responseData = await response.Content.ReadAsStringAsync(cancellationToken);

            CheckResponseData(request!, url, responseData);

            return JsonConvert.DeserializeObject<TResponse>(responseData);
        }
        catch (ExceptionsManager ex)
        {
            _logger.LogError(ex, $"Request to {url} failed");
            throw;
        }
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending request to {Url}", url);
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content, cancellationToken);
            await EnsureSuccessStatusCode(response);
            _logger.LogInformation("Request to {Url} successfull", url);
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken);

            CheckResponseData(request!, url, responseData);

            return JsonConvert.DeserializeObject<TResponse>(responseData);
        }
        catch (ExceptionsManager ex)
        {
            _logger.LogError(ex, $"Request to {url} failed");
            throw;
        }
    }

    private void CheckResponseData(object request, string url, string responseData)
    {
        if (string.IsNullOrEmpty(responseData))
        {
            _logger.LogError($"Response is empty with request: {JsonConvert.SerializeObject(request)} and with url: {url}");
            throw new ExceptionsManager(System.Net.HttpStatusCode.NoContent, $"Requested url Response is empty, url: {url}");
        }
    }

    public async Task DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        await EnsureSuccessStatusCode(response);
    }

    private async Task EnsureSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new ExceptionsManager(response.StatusCode, $"Request faild with status code {response.StatusCode} :{content} ");
        }
    }
}