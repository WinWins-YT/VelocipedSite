using VelocipedSite.DAL.Infrastructure;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalInfrastructure(this IServiceCollection services, IConfigurationRoot config)
    {
        services.Configure<DalOptions>(config.GetSection(nameof(DalOptions)));
        
        Postgres.AddMigrations(services);
        
        return services;
    }
}