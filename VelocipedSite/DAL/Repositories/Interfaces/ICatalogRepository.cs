using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface ICatalogRepository : IDbRepository
{
    Task<CatalogEntityV1[]> Query(CatalogQueryModel query, CancellationToken token = default);
    Task<CatalogEntityV1> QueryById(CatalogQueryModel query, CancellationToken token = default);
}