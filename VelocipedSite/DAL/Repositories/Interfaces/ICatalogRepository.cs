using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface ICatalogRepository : IDbRepository
{
    Task<CatalogEntity_V1[]> Query(CatalogQueryModel query, CancellationToken token = default);
}