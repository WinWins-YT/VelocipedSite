using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Repositories;

public class CatalogRepository : BaseRepository, ICatalogRepository
{
    public CatalogRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<CatalogEntity_V1[]> Query(CatalogQueryModel query, CancellationToken token = default)
    {
        const string sqlQuery = """
                                SELECT id, shop_id, name, path_to_img FROM categories
                                WHERE shop_id = @ShopId
                                """;

        var sqlQueryParams = new
        {
            ShopId = query.ShopId
        };

        await using var connection = await OpenConnection();
        var categories = await connection.QueryAsync<CatalogEntity_V1>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token));

        return categories.ToArray();
    }

    public async Task<CatalogEntity_V1> QueryById(CatalogQueryModel query, CancellationToken token = default)
    {
        const string sqlQuery = """
                                SELECT id, shop_id, name, path_to_img FROM categories
                                WHERE id = @Id AND (shop_id = @ShopId OR @ShopId IS NULL)
                                """;

        var sqlQueryParams = new
        {
            Id = query.Id,
            ShopId = query.ShopId
        };

        await using var connection = await OpenConnection();
        var category = await connection.QueryAsync<CatalogEntity_V1>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token));

        return category.Single();
    }
}