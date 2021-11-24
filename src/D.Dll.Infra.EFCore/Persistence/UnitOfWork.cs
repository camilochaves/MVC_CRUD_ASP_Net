
using System;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infra.EFCore.Persistence.Repository;

namespace Infra.EFCore.Persistence
{
  public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
  {
    private readonly ApplicationContext _context;
    public UnitOfWork(ApplicationContext context)
    {
      _context = context;
      Employees = new EmployeeRepository(_context);
    }
    public IEmployeeRepository Employees { get; private set; }
    public int Complete()
    {
      return _context.SaveChanges();
    }
    public void Dispose() => _context.Dispose();

    public ValueTask DisposeAsync() => _context.DisposeAsync();
  }
}