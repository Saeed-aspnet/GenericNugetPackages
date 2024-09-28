namespace NugetPackage.GenericHttpClient.Interfaces;

public interface IHttpClientService
{
    void AddAuthorizationHeader(string token);
    void RemoveAuthorizationHeader();
    Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken);
    Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken cancellationToken);
    Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(string url);
}