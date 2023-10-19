using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IProductsRepository : IDbRepository
{
    Task<ProductEntity_V1[]> Query(ProductsQueryModel query, CancellationToken token = default);
    Task<ProductEntity_V1> QueryById(ProductQueryByIdModel query, CancellationToken token = default);
}