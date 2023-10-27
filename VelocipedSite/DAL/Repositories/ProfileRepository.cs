using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.DAL.Repositories;

public class ProfileRepository : BaseRepository, IProfileRepository
{
    public ProfileRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<TokenEntity_V1> GetToken(TokenQuery query, CancellationToken cancellationToken = default)
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

    public async Task<UserEntity_V1> GetUserFromToken(TokenQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    SELECT * FROM users
                                    WHERE id=(SELECT user_id FROM tokens
                                                                WHERE token = @Token)
                                    """;

            var sqlQueryParam = new
            {
                Token = query.Token
            };

            await using var connection = await OpenConnection();
            var user = await connection.QueryAsync<UserEntity_V1>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return user.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No user found", exception);
        }
    }

    public async Task<long[]> RemoveToken(TokenQuery query, CancellationToken cancellationToken = default)
    {
        const string sqlQuery = """
                                DELETE FROM tokens
                                WHERE token = @Token
                                RETURNING id
                                """;

        var sqlQueryParam = new
        {
            Token = query.Token
        };

        await using var connection = await OpenConnection();
        var rows = (await connection.QueryAsync<long>(new CommandDefinition(sqlQuery, sqlQueryParam, 
            cancellationToken: cancellationToken))).ToArray();

        if (rows.Length == 0)
            throw new EntityNotFoundException("No token found to invalidate");

        return rows;
    }
}