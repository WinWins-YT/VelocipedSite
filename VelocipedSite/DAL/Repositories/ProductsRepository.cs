using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Exceptions;
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
        var products = (await connection.QueryAsync<ProductEntity_V1>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token))).ToArray();

        if (products.Length == 0)
            throw new EntityNotFoundException("No products found in this catalog category");
        return products;
    }

    public async Task<ProductEntity_V1> QueryById(ProductQueryByIdModel query, CancellationToken token = default)
    {
        const string sqlQuery = """
                                SELECT id, shop_id, category_id, name, description, path_to_img, price FROM products
                                WHERE id = @ProductId AND shop_id = @ShopId AND category_id = @CategoryId
                                """;

        var sqlQueryParams = new
        {
            ProductId = query.ProductId,
            ShopId = query.ShopId,
            CategoryId = query.CategoryId
        };

        await using var connection = await OpenConnection();
        var product = await connection.QueryAsync<ProductEntity_V1>(
            new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: token));

        return product.Single();
    }
}