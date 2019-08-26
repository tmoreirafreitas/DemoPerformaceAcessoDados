using Microsoft.CodeAnalysis.Options;
using System;
using System.Data;
using System.Text;

namespace Demo.Performace.Acesso.Dados.AdoNet
{
    public class ConnectionFactory : IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly ConnectionSetting _connSettings;

        public ConnectionFactory(Option<ConnectionSetting> settings, IDbConnection connection)
        {
            _connSettings = settings.DefaultValue;
            _connection = connection;
        }

        public IDbConnection CreateConnection()
        {
            var sb = new StringBuilder();
            sb.Append(_connSettings.Server);
            sb.Append(_connSettings.Database);
            sb.Append(_connSettings.User);
            sb.Append(_connSettings.Password);
            sb.Append(_connSettings.MultipleActiveResultSets);
            _connection.ConnectionString = sb.ToString();

            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            return _connection;
        }

        public void Fechar()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Fechar();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}