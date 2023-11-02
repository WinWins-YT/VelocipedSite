using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Npgsql;
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

    public async Task<TokenEntityV1> GetToken(TokenQuery query, CancellationToken cancellationToken = default)
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
            var token = await connection.QueryAsync<TokenEntityV1>(
                new CommandDefinition(sqlQuery, sqlQueryParams, cancellationToken: cancellationToken));

            return token.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No token found", exception);
        }
    }

    public async Task<UserEntityV1> GetUserFromToken(TokenQuery query, CancellationToken cancellationToken = default)
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
            var user = await connection.QueryAsync<UserEntityV1>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return user.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No user found by this token", exception);
        }
    }

    public async Task<UserEntityV1> GetUserByEmail(EmailQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    SELECT * FROM users
                                    WHERE email=@Email
                                    """;

            var sqlQueryParam = new
            {
                Email = query.Email
            };

            await using var connection = await OpenConnection();
            var user = await connection.QueryAsync<UserEntityV1>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return user.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No user found with this email", exception);
        }
    }

    public async Task<TokenEntityV1> CreateTokenForUser(UserIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    INSERT INTO tokens (token, user_id, valid_until)
                                    VALUES (gen_random_uuid(), @UserId, @ValidUntil)
                                    RETURNING token;
                                    """;

            var sqlQueryParam = new
            {
                UserId = query.UserId,
                ValidUntil = DateTime.UtcNow.AddDays(7)
            };

            await using var connection = await OpenConnection();
            var token = await connection.QueryAsync<TokenEntityV1>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return token.Single();
        }
        catch (PostgresException exception)
        {
            if (exception.SqlState == PostgresErrorCodes.UniqueViolation)
                throw new EntityAlreadyExistsException("Token for this user ID is already exists", exception);

            throw;
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

    public async Task<long> AddUser(UserQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    INSERT INTO users (activated, email, password, first_name, last_name, address, phone)
                                    VALUES (FALSE, @Email, @Password, @FirstName, @LastName, @Address, @Phone)
                                    RETURNING id;
                                    """;

            var sqlQueryParam = new
            {
                Email = query.Email,
                Password = query.Password,
                FirstName = query.FirstName,
                LastName = query.LastName,
                Address = query.Address,
                Phone = query.Phone
            };

            await using var connection = await OpenConnection();
            var userId = await connection.QueryAsync<long>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return userId.Single();
        }
        catch (PostgresException exception)
        {
            if (exception.SqlState == PostgresErrorCodes.UniqueViolation)
                throw new EntityAlreadyExistsException("User with this e-mail already exists", exception);
            
            throw;
        }
    }

    public async Task<long> RemoveUser(UserIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    DELETE FROM users
                                    WHERE id=@UserId
                                    RETURNING id
                                    """;

            var sqlQueryParam = new
            {
                UserId = query.UserId
            };

            await using var connection = await OpenConnection();
            var user = await connection.QueryAsync<long>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return user.Single();
        }
        catch (InvalidOperationException e)
        {
            throw new EntityNotFoundException("No user found by this ID", e);
        }
    }

    public async Task<long> ActivateUser(UserIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    UPDATE users
                                    SET activated=TRUE
                                    WHERE id=@Id
                                    RETURNING id
                                    """;

            var sqlQueryParam = new
            {
                Id = query.UserId
            };

            await using var connection = await OpenConnection();
            var user = await connection.QueryAsync<long>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return user.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No user found by this ID", exception);
        }
    }

    public async Task<long> UpdateUser(UpdateUserQuery query, CancellationToken cancellationToken = default)
    {
        try
        {

            const string sqlQuery = """
                                    UPDATE users
                                    SET first_name = @FirstName, last_name = @LastName, address = @Address, phone = @Phone
                                    WHERE id = @Id
                                    RETURNING id;
                                    """;

            var sqlQueryParam = new
            {
                FirstName = query.FirstName,
                LastName = query.LastName,
                Address = query.Address,
                Phone = query.Phone,
                Id = query.UserId
            };

            await using var connection = await OpenConnection();
            var user = await connection.QueryAsync<long>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return user.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No user found this ID", exception);
        }
    }

    public async Task<long> ChangeUserPassword(ChangePasswordQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    UPDATE users
                                    SET password = @Password
                                    WHERE id=@UserId
                                    RETURNING id;
                                    """;

            var sqlQueryParam = new
            {
                Password = query.Password,
                UserId = query.UserId
            };

            await using var connection = await OpenConnection();
            var user = await connection.QueryAsync<long>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return user.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No user found by this ID", exception);
        }
    }

    public async Task<long[]> RemoveExpiredTokens(CancellationToken cancellationToken = default)
    {
        const string sqlQuery = """
                                DELETE FROM tokens
                                WHERE valid_until < current_timestamp;
                                """;

        await using var connection = await OpenConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(sqlQuery, cancellationToken: cancellationToken));

        return ids.ToArray();
    }
}