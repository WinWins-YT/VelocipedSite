using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Repositories;

public class ProductsRepository : BaseRepository, IProductsRepository
{
    public ProductsRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<ProductEntity_V1[]> Query(ProductsQueryModel query, CancellationToken token = default)
    {
        const string sqlQuery = """
                                SELECT id, shop_id, category_id, name, description, path_to_img, price FROM products
                                WHERE category_id = @CategoryId AND shop_id = @ShopId
                                """;

        var sqlQueryParams = new
        {
            CategoryId = query.CategoryId,
            ShopId = query.ShopId
        };

        await using var connection = await OpenConnection();
        var products = await connection.QueryAsync<ProductEntity_V1>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token));

        return products.ToArray();
    }
}