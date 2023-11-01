using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IOrdersRepository : IDbRepository
{
    Task<long> CreateOrder(OrderQuery query, CancellationToken cancellationToken = default);
}