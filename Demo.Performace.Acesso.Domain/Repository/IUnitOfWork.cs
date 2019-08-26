using System.Data;

namespace Demo.Performace.Acesso.Domain.Repository
{
    public interface IUnitOfWork
    {
        IDbConnection GetConnection();
        IDbTransaction GetTransaction();
        void Commit();
        void Rollback();
    }
}
