using System.Transactions;
using Npgsql;
using VelocipedSite.DAL.Infrastructure;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.DAL.Settings;

namespace VelocipedSite.DAL.Repositories;

public class BaseRepository : IDbRepository
{
    private readonly DalOptions _dalOptions;

    protected BaseRepository(DalOptions dalOptions)
    {
        _dalOptions = dalOptions;
    }

    protected static async Task<NpgsqlConnection> OpenConnection()
    {
        var connection = await Postgres.DataSource.OpenConnectionAsync();
        await connection.ReloadTypesAsync();
        return connection;
    }
    
    public TransactionScope CreateTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = level,
                Timeout = TimeSpan.FromSeconds(5)
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}