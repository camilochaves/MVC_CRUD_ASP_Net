using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infra.Dapper.Repository
{
    public class GenericRepository : IGenericRepository<Employee>
    {
        public Task AddAsync(Employee entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Employee> Find(Expression<Func<Employee, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Remove(Employee entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}