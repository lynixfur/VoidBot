namespace Void.Core
{
    using System;
    using Void.Core.Repositories;

    public interface IUnitOfWork : IDisposable
    {
        IServerConfigs ServerConfigs { get; }
        void Commit();
    }
}