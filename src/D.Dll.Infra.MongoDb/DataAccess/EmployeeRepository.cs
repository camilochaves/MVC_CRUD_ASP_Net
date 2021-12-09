using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infra.MongoDb
{
    public class EmployeeRepository: IEmployeeRepository, IDisposable
    {
        private readonly MongoContext _context;

        public EmployeeRepository(MongoContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(Employee entity)
        {
            await _context.Employees().InsertOneAsync(entity);
            return;
        }

        public async Task AddRangeAsync(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => _context.Dispose();

        public IQueryable<Employee> Find(Expression<Func<Employee, bool>> expression)
        {
            var result =  _context.Employees().AsQueryable<Employee>().Where(expression);
            return result;
        }

        public async Task<Employee> FindAsync(int id)
        {
            var result =  await _context.Employees().FindAsync(x=>x.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var results = await _context.Employees().FindAsync( _ => true);
            return results.ToList();
        }

        public async Task<Employee> GetByBadgeNumberAsync(int? employeeNumber)
        {
            var result =  await _context.Employees().FindAsync(x=>x.EmployeeNumber == employeeNumber);
            return result.FirstOrDefault();
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            var result =  await _context.Employees().FindAsync(x=>x.Email == email);
            return result.FirstOrDefault();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var result =  await _context.Employees().FindAsync(x=>x.Id == id);
            return result.FirstOrDefault();
        }

        public void Remove(Employee entity)
        {
            var result = _context.Employees().DeleteOne(x=>x.Id == entity.Id);
        }

        public void RemoveRange(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Employee entity)
        {
            var filter = Builders<Employee>.Filter.Eq("Id", entity.Id);
            _context.Employees().ReplaceOne(filter, entity, new ReplaceOptions{IsUpsert = true});
        }
    }
}