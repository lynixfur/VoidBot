namespace Void.Data.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Void.Core.Entities;
    using Void.Core.Repositories;

    internal class ServerConfigs : RepositoryBase, IServerConfigs
    {
        public ServerConfigs(IDbTransaction _transaction) : base(_transaction)
        {

        }

        public Task<IEnumerable<ServerConfig>> GetAllActiveAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<ServerConfig> GetAsync(ulong _key)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(ServerConfig _entity)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(ServerConfig _entity)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(ulong _key)
        {
            throw new System.NotImplementedException();
        }
    }
}
