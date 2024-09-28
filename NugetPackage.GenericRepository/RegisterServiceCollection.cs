using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NugetPackage.GenericRepository.Interfaces;

namespace NugetPackage.GenericRepository;
public static class RegisterServiceCollection
{
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}