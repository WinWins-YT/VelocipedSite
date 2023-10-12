using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IShopsRepository : IDbRepository
{
    Task<ShopEntityV1[]> QueryAll(CancellationToken token = default);
    Task<ShopEntityV1> QueryById(ShopsQueryModel query, CancellationToken token = default);
}