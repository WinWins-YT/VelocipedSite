using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IShopsRepository : IDbRepository
{
    Task<ShopEntity_V1[]> QueryAll(CancellationToken token = default);
    Task<ShopEntity_V1> QueryById(ShopsQueryModel query, CancellationToken token = default);
}