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
    public class MongoContext: IEmployeeRepository, IDisposable
    {
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "myDb";
        private const string EmployeeCollection = "employees";
        private MongoClient client;
        private IMongoDatabase db;
        private IMongoCollection<Employee> collection;
        
        public MongoContext()
        {
            this.client = new MongoClient(ConnectionString);
            this.db = client.GetDatabase(DatabaseName);
            this.collection = db.GetCollection<Employee>(EmployeeCollection);
        }

        public async Task AddAsync(Employee entity)
        {
            await collection.InsertOneAsync(entity);
            return;
        }

        public async Task AddRangeAsync(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            client = null;
            db = null;
        }

        public IQueryable<Employee> Find(Expression<Func<Employee, bool>> expression)
        {
            var result =  collection.AsQueryable<Employee>().Where(expression);
            return result;
        }

        public async Task<Employee> FindAsync(int id)
        {
            var result =  await collection.FindAsync(x=>x.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var results = await collection.FindAsync( _ => true);
            return results.ToList();
        }

        public async Task<Employee> GetByBadgeNumberAsync(int? employeeNumber)
        {
            var result =  await collection.FindAsync(x=>x.EmployeeNumber == employeeNumber);
            return result.FirstOrDefault();
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            var result =  await collection.FindAsync(x=>x.Email == email);
            return result.FirstOrDefault();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var result =  await collection.FindAsync(x=>x.Id == id);
            return result.FirstOrDefault();
        }

        public void Remove(Employee entity)
        {
            var result = collection.DeleteOne(x=>x.Id == entity.Id);
        }

        public void RemoveRange(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Employee entity)
        {
            var filter = Builders<Employee>.Filter.Eq("Id", entity.Id);
            collection.ReplaceOne(filter, entity, new ReplaceOptions{IsUpsert = true});
        }
    }
}