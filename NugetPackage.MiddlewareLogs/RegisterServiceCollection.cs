using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NugetPackage.MiddlewareLogs.Contracts;

namespace NugetPackage.MiddlewareLogs;
public static class RegisterServiceCollection
{
    public static IServiceCollection AddLogMiddleware(this IServiceCollection services, IConfiguration configuration, string sectionName = "MiddlewareConfig")
    {
        services.AddOptions();
        services.Configure<MiddlewareConfig>(configuration.GetSection(sectionName));

        return services;
    }

    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}