using VelocipedSite.DAL.Infrastructure;
using VelocipedSite.DAL.Repositories;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalRepositories(this IServiceCollection services)
    {
        services.AddScoped<IShopsRepository, ShopsRepository>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddDalInfrastructure(this IServiceCollection services, IConfigurationRoot config)
    {
        services.Configure<DalOptions>(config.GetSection(nameof(DalOptions)));
        
        Postgres.MapCompositeTypes(services);
        Postgres.AddMigrations(services);
        
        return services;
    }
}