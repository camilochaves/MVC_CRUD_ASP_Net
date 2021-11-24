using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
  public interface IEmployeeRepository : IGenericRepository<Employee>
  {
    public Task<Employee> GetByIdAsync(int id);
    public Task<Employee> GetByEmailAsync(string email);
    public Task<Employee> GetByBadgeNumberAsync(int? employeeNumber);
  }
}