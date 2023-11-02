using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IProductsRepository : IDbRepository
{
    Task<ProductEntityV1[]> Query(ProductsQueryModel query, CancellationToken token = default);
    Task<ProductEntityV1> QueryById(ProductQueryByIdModel query, CancellationToken token = default);
}