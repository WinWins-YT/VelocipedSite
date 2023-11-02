using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Repositories;

public class CatalogRepository : BaseRepository, ICatalogRepository
{
    public CatalogRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<CatalogEntityV1[]> Query(CatalogQueryModel query, CancellationToken token = default)
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
        var categories = (await connection.QueryAsync<CatalogEntityV1>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token))).ToArray();

        if (categories.Length == 0)
            throw new EntityNotFoundException("No catalog categories found by this Shop ID");
        return categories;
    }

    public async Task<CatalogEntityV1> QueryById(CatalogQueryModel query, CancellationToken token = default)
    {
        try
        {
            const string sqlQuery = """
                                    SELECT id, shop_id, name, path_to_img FROM categories
                                    WHERE id = @Id AND (shop_id = @ShopId OR (@ShopId = '') IS NOT FALSE)
                                    """;

            var sqlQueryParams = new
            {
                Id = query.Id,
                ShopId = query.ShopId
            };

            await using var connection = await OpenConnection();
            var category = await connection.QueryAsync<CatalogEntityV1>(
                new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token));

            return category.Single();
        }
        catch (InvalidOperationException ex)
        {
            throw new EntityNotFoundException("No catalog category found by this ID", ex);
        }
    }
}