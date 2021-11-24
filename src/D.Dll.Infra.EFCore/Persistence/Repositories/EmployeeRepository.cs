using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.EFCore.Persistence.Repository
{
  public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
  {
    public EmployeeRepository(ApplicationContext context) : base(context)
    {

    }

    public async Task<Employee> GetByIdAsync(int id)
    {
      return await _context.Employees.FirstOrDefaultAsync<Employee>(x=>x.Id == id);
    }
    public async Task<Employee> GetByEmailAsync(string email)
    {
      return await _context.Employees.FirstOrDefaultAsync<Employee>(x => x.Email == email);
    }

    public async Task<Employee> GetByBadgeNumberAsync(int? employeeNumber)
    {
      return await _context.Employees.FirstOrDefaultAsync<Employee>(x => x.EmployeeNumber == employeeNumber);
    }

    
  }
}