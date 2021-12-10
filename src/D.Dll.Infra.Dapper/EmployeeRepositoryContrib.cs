using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DapperDemo.Repository
{
    public class CompanyRepositoryContib : IEmployeeRepository
    {
        private IDbConnection db;

        public CompanyRepositoryContib(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task AddAsync(Employee entity)
        {
            var id = await db.InsertAsync(entity);
            entity.Id = (int)id;
            return;
        }

        public Task AddRangeAsync(IEnumerable<Employee> entities)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Employee> Find(Expression<System.Func<Employee, bool>> expression)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee> FindAsync(int id)
        {
            return await db.GetAsync<Employee>(id);
        }

        public List<Employee> GetAll()
        {
            return db.GetAll<Employee>().ToList();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return (await db.GetAllAsync<Employee>()).ToList();
        }

        public Task<Employee> GetByBadgeNumberAsync(int? employeeNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<Employee> GetByEmailAsync(string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<Employee> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(Employee entity)
        {
            db.Delete(entity);
        }

        public void RemoveRange(IEnumerable<Employee> entities)
        {
            throw new System.NotImplementedException();
        }

        public Employee Update(Employee entity)
        {
            db.Update(entity);
            return entity;
        }

        void IGenericRepository<Employee>.Update(Employee entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
