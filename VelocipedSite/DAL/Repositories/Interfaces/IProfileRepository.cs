using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<TokenEntity_V1> GetTokenForUser(TokenForUserQuery query, CancellationToken cancellationToken = default);
}