using System.Text;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Models;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<TokenEntity_V1> GetToken(TokenQuery query, CancellationToken cancellationToken = default);
    Task<UserEntity_V1> GetUserFromToken(TokenQuery query, CancellationToken cancellationToken = default);
    Task<UserEntity_V1> GetUserByEmail(EmailQuery query, CancellationToken cancellationToken = default);
    Task<TokenEntity_V1> CreateTokenForUser(CreateTokenQuery query, CancellationToken cancellationToken = default);
    Task<long[]> RemoveToken(TokenQuery query, CancellationToken cancellationToken = default);
    Task<long> AddUser(UserQuery query, CancellationToken cancellationToken = default);
    Task<long[]> RemoveExpiredTokens(CancellationToken cancellationToken = default);
}