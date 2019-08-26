using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Demo.Performace.Acesso.Dados.AdoNet
{
    public class Repository<T> where T : class
    {
        private readonly IDbCommand _command;
        public Repository(ConnectionFactory factory)
        {
            var connection = factory.CreateConnection();
            _command = connection.CreateCommand();
        }
        protected IDataReader ExecuteReader(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                DefineQuery(query, parameters);
                return _command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected int ExecuteCommand(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                DefineQuery(query, parameters);
                return Convert.ToInt32(_command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected Task<IDataReader> ExecuteReaderAsync(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                DefineQuery(query, parameters);
                return Task.FromResult(_command.ExecuteReader());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected Task<int> ExecuteCommandAsync(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                DefineQuery(query, parameters);
                return Task.FromResult(Convert.ToInt32(_command.ExecuteScalar()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DefineQuery(string query, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException("É preciso definir uma query para execução da instrução SQL.");
            _command.CommandText = query;
            if (parameters != null)
            {
                foreach (var (key, value) in parameters)
                {
                    var parameter = _command.CreateParameter();
                    parameter.ParameterName = key;
                    parameter.Value = value;
                    _command.Parameters.Add(parameter);
                }
            }
        }

        public T PopulateToSingle<T>(IDataReader reader) where T : new()
        {            
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";
            if (reader == null || reader.IsClosed)
                throw new ArgumentNullException(msg);
            return reader.MapToSingle<T>();
        }

        public IList<T> PopulateToList<T>(IDataReader reader) where T : new()
        {
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";
            if (reader == null || reader.IsClosed)
                throw new ArgumentNullException(msg);
            return reader.MapToList<T>();
        }
    }
}