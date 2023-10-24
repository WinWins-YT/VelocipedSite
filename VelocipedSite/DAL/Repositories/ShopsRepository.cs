using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Repositories;

public class ShopsRepository : BaseRepository, IShopsRepository
{
    public ShopsRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<ShopEntityV1[]> QueryAll(CancellationToken token)
    {
        const string sqlQuery = "SELECT id, shop_id, name, path_to_img, min_price FROM shops";

        await using var connection = await OpenConnection();
        var shops = await connection.QueryAsync<ShopEntityV1>(
            new CommandDefinition(sqlQuery, cancellationToken: token));

        return shops.ToArray();
    }

    public async Task<ShopEntityV1> QueryById(ShopsQueryModel query, CancellationToken token)
    {
        try
        {
            const string sqlQuery = """
                                    SELECT id, shop_id, name, path_to_img, min_price FROM shops
                                    WHERE shop_id = @ShopId
                                    """;

            var sqlQueryParams = new
            {
                ShopId = query.ShopId
            };

            await using var connection = await OpenConnection();
            var shop = await connection.QueryAsync<ShopEntityV1>(
                new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token));

            return shop.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No shop found by this ID");
        }
    }
}