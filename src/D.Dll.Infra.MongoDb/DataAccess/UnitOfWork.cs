using System;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;

namespace Infra.MongoDb
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MongoContext _context;
        public UnitOfWork(MongoContext context)
        {
            _context = context;
            Employees = new EmployeeRepository(_context);
        }
        public IEmployeeRepository Employees { get; private set; }
        public int Complete() => _context.SaveChanges();
        public void Dispose() => _context.Dispose();
    }
}