using Dapper;
using Microsoft.Extensions.Options;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Repositories;

public class PromocodesRepository : BaseRepository, IPromocodesRepository
{
    public PromocodesRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<PromocodeEntityV1> GetPromocode(GetPromocodeQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sqlQuery = """
                                    SELECT * FROM promocodes
                                    WHERE code=@Code
                                    """;

            var sqlQueryParam = new
            {
                Code = query.Code
            };

            await using var connection = await OpenConnection();
            var promo = await connection.QueryAsync<PromocodeEntityV1>(
                new CommandDefinition(sqlQuery, sqlQueryParam, cancellationToken: cancellationToken));

            return promo.Single();
        }
        catch (InvalidOperationException exception)
        {
            throw new EntityNotFoundException("No promocode found", exception);
        }
    }
}