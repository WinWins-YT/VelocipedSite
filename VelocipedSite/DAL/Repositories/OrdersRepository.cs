using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Repositories;

public class OrdersRepository : BaseRepository, IOrdersRepository
{
    public OrdersRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<long> CreateOrder(OrderQuery query, CancellationToken cancellationToken = default)
    {
        const string sqlQuery = """
                                INSERT INTO orders (status, user_id, date, address, phone, products)
                                VALUES ('created', @UserId, current_timestamp, @Address, @Phone, @Products)
                                RETURNING id;
                                """;

        var sqlQueryParam = new
        {
            UserId = query.UserId,
            Address = query.Address,
            Phone = query.Phone,
            Products = query.Products
        };

        await using var connection = await OpenConnection();
        var orderId = await connection.QueryAsync<long>(
            new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

        return orderId.Single();
    }
}