using System;
using DnsClient.Internal;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infra.MongoDb
{
    public class MongoContext : IDisposable
    {
        private readonly ILogger<MongoContext> logger;
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Employee> _employees;

        public MongoContext(
            ILogger<MongoContext> logger,
            string db = "myDb",
            string user = "camilo",
            string password = "password")
        {
            this.logger = logger;
            var mongoUrl = new MongoUrl($"mongodb://{user}:{password}@localhost:27017");
            this._client = new MongoClient(mongoUrl);
            if (_client != null)
            {
                this._database = _client.GetDatabase(db);
                if (_database is null) logger.LogError($"Database {db} not Found!");
                this._employees = _database.GetCollection<Employee>("Employees");
                if (_employees is null && _database is not null)
                {
                    _database.CreateCollection("Employees");
                    _employees = _database.GetCollection<Employee>("Employees");
                }
            }
            else
            {
                logger.LogError("Mongo Client not created!");
            }
        }

        public IClientSession Session { get; set; }

        public IMongoCollection<Employee> Employees() => _employees;

        public int SaveChanges() => 1;

        public void Dispose()
        {
            Session?.Dispose();
            _client = null;
            _database = null;
        }

    }
}