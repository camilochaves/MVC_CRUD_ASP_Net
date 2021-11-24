using Domain.Entities;
using Infra.EFCore.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infra.EFCore
{
  public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
             base.OnModelCreating(modelBuilder);
        }
    }
}