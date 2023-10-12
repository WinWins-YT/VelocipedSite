using FluentMigrator.Runner;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.NameTranslation;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Infrastructure;

public class Postgres
{
    public static NpgsqlDataSource DataSource { get; private set; }

    public static void MapCompositeTypes(IServiceCollection services)
    {
        var cfg = services.BuildServiceProvider().GetRequiredService<IOptions<DalOptions>>();
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(cfg.Value.ConnectionString);
        dataSourceBuilder.MapComposite<ShopEntityV1>("shop_v1", new NpgsqlSnakeCaseNameTranslator());

        DataSource = dataSourceBuilder.Build();
    }
    
    public static void AddMigrations(IServiceCollection services)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb.AddPostgres()
                .WithGlobalConnectionString(s =>
                {
                    var cfg = s.GetRequiredService<IOptions<DalOptions>>();
                    return cfg.Value.ConnectionString;
                })
                .ScanIn(typeof(Postgres).Assembly).For.Migrations()
            );
    }
}