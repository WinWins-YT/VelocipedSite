using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Exceptions;
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
                                INSERT INTO orders (status, user_id, date, address, phone, products, total_price, sale_value)
                                VALUES ('created', @UserId, current_timestamp, @Address, @Phone, CAST(:Products as product_v1[]), @TotalPrice, @SaleValue)
                                RETURNING id;
                                """;

        var sqlQueryParam = new
        {
            UserId = query.UserId,
            Address = query.Address,
            Phone = query.Phone,
            Products = query.Products,
            TotalPrice = query.TotalPrice,
            SaleValue = query.SaleValue
        };

        await using var connection = await OpenConnection();
        var orderId = await connection.QueryAsync<long>(
            new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

        return orderId.Single();
    }

    public async Task<OrderEntityV1[]> GetOrdersForUser(UserIdQuery query, CancellationToken cancellationToken = default)
    {
        const string sqlQuery = """
                                SELECT * FROM orders
                                WHERE user_id=@UserId;
                                """;

        var sqlQueryParam = new
        {
            UserId = query.UserId
        };

        await using var connection = await OpenConnection();
        var orders = await connection.QueryAsync<OrderEntityV1>(
            new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

        return orders.ToArray();
    }

    public async Task<OrderEntityV1> GetOrderById(OrderIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    SELECT * FROM orders
                                    WHERE id=@OrderId
                                    """;

            var sqlQueryParam = new
            {
                OrderId = query.OrderId
            };

            await using var connection = await OpenConnection();
            var order = await connection.QueryAsync<OrderEntityV1>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return order.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No order found by this Id", exception);
        }
    }

    public async Task<long> CancelOrder(CancelOrderQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    UPDATE orders
                                    SET status='cancelled'
                                    WHERE id=@OrderId AND user_id=@UserId
                                    RETURNING id;
                                    """;

            var sqlQueryParam = new
            {
                OrderId = query.OrderId,
                UserId = query.UserId
            };

            await using var connection = await OpenConnection();
            var orderId = await connection.QueryAsync<long>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return orderId.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No order found by this Id", exception);
        }
    }

    public async Task<long> ConfirmOrderPayed(OrderIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    UPDATE orders
                                    SET status='payed'
                                    WHERE id=@OrderId
                                    RETURNING id;
                                    """;

            var sqlQueryParam = new
            {
                OrderId = query.OrderId
            };

            await using var connection = await OpenConnection();
            var orderId = await connection.QueryAsync<long>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return orderId.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No order found by this Id", exception);
        }
    }
}