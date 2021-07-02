namespace Void.Data.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;

    internal abstract class RepositoryBase
    {
        private readonly IDbTransaction transaction;
        private IDbConnection Connection { get { return transaction.Connection; } }

        public RepositoryBase(IDbTransaction _transaction)
        {
            this.transaction = _transaction;
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string _sql, object _param)
        {
            return await Connection.QueryFirstOrDefaultAsync<T>(_sql, _param, transaction);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string _sql, object _param = null)
        {
            return await Connection.QueryAsync<T>(_sql, _param, transaction);
        }

        protected async Task ExecuteAsync(string _sql, object _param)
        {
            await Connection.ExecuteAsync(_sql, _param, transaction);
        }
    }
}