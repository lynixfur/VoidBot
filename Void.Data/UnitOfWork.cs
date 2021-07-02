namespace Void.Data
{
    using System;
    using System.Data;
    using Npgsql;
    using Void.Core;
    using Void.Core.Entities;
    using Void.Core.Repositories;
    using Void.Data.Repositories;

    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        private IServerConfigs serverConfigs;
        private bool disposed;

        public UnitOfWork(Config _config, int _dbIndex)
        {
            connection = new NpgsqlConnection(_config.MySql[_dbIndex].GetConnectionString());
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        public IServerConfigs ServerConfigs
        {
            get
            {
                return serverConfigs ??= new ServerConfigs(transaction);
            }
        }

        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
                ResetRepositories();
                transaction = connection.BeginTransaction();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region Private Methods
        private void ResetRepositories()
        {
            serverConfigs = null;
        }

        private void Dispose(bool _disposing)
        {
            if (disposed) return;
            if (_disposing)
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                    transaction = null;
                }
                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }
            disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion
    }
}
