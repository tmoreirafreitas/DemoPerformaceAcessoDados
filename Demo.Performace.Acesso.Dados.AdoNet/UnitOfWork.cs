using Demo.Performace.Acesso.Domain.Repository;
using System;
using System.Data;

namespace Demo.Performace.Acesso.Dados.AdoNet
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        public UnitOfWork(ConnectionFactory factory)
        {
            _connection = factory.CreateConnection();
        }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _connection.Close();
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _transaction.Dispose();
                _connection.Close();
            }
        }

        public IDbConnection GetConnection()
        {
            return _connection;
        }

        public IDbTransaction GetTransaction()
        {
            _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
            return _transaction;
        }

    }
}
