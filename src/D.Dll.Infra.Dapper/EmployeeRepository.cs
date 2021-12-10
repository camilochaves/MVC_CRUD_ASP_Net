using Dapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DapperDemo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDbConnection db;

        public EmployeeRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Employee Add(Employee company)
        {
            var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                        + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = db.Query<int>(sql,company).Single();
            company.CompanyId = id;
            return company;

        }

        public Employee Find(int id)
        {
            var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId";
            return db.Query<Employee>(sql, new { @CompanyId = id }).Single();
        }

        public List<Employee> GetAll()
        {
            var sql = "SELECT * FROM Companies";
            return db.Query<Employee>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            db.Execute(sql, new { id });
        }

        public Employee Update(Employee company)
        {
            var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, " +
                "State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            db.Execute(sql, company);
            return company;
        }
    }
}
