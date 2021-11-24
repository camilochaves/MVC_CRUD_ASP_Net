using System;
using Domain.Interfaces.Repositories;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IEmployeeRepository Employees { get; }
        int Complete();
    }
}