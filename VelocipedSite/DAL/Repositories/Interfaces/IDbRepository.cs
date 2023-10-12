using System.Transactions;

namespace VelocipedSite.DAL.Repositories.Interfaces;

public interface IDbRepository
{
    public TransactionScope CreateTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted);
}