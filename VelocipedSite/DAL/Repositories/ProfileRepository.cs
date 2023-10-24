using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.DAL.Repositories;

public class ProfileRepository : BaseRepository, IProfileRepository
{
    public ProfileRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<TokenEntity_V1> GetTokenForUser(TokenForUserQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    SELECT * FROM tokens
                                    WHERE token = @Token
                                    """;

            var sqlQueryParams = new
            {
                Token = query.Token
            };

            await using var connection = await OpenConnection();
            var token = await connection.QueryAsync<TokenEntity_V1>(
                new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: cancellationToken));

            return token.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No token found", exception);
        }
    }
}