using GenericHttpClient.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NugetPackage.GenericHttpClient.Interfaces;
using NugetPackage.GenericHttpClient.Services;

namespace GenericHttpClient;
public static class RegisterServiceCollection
{
    public static IServiceCollection AddGenericHttpClient(this IServiceCollection services, IConfiguration configuration, string sectionName = "HttpClientOptions")
    {
        var config = configuration.GetSection(sectionName).Get<HttpClientOptions>();
        if (config is null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        services.AddTransient<IHttpClientService, HttpClientService>();
        services.AddHttpClient<HttpClientService>("HttpClientService", (serviceProvider, client) =>
        {
            if (!string.IsNullOrWhiteSpace(config.BaseAddress))
            {
                client.BaseAddress = new Uri(config.BaseAddress);
                client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
            }
            if (config.DefaultHeaders.Count != 0)
            {
                foreach (var header in config.DefaultHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }).SetHandlerLifetime(TimeSpan.FromMinutes(config.HttpClientLifeTime));

        return services;
    }
}