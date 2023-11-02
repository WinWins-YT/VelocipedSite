using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IOrdersRepository : IDbRepository
{
    Task<long> CreateOrder(OrderQuery query, CancellationToken cancellationToken = default);
    Task<OrderEntityV1[]> GetOrdersForUser(UserIdQuery query, CancellationToken cancellationToken = default);
    Task<OrderEntityV1> GetOrderById(OrderIdQuery query, CancellationToken cancellationToken = default);
    Task<long> CancelOrder(CancelOrderQuery query, CancellationToken cancellationToken = default);
    Task<long> ConfirmOrderPayed(OrderIdQuery query, CancellationToken cancellationToken = default);
}