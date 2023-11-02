using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IPromocodesRepository : IDbRepository
{
    Task<PromocodeEntityV1> GetPromocode(GetPromocodeQuery query, CancellationToken cancellationToken = default);
}