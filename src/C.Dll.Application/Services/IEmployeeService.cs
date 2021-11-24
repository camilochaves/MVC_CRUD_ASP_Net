using System.Threading.Tasks;
using Application.InputModels;
using Application.Wrappers.Response;
using Domain.Entities;

namespace Application.Services.Interfaces
{
  public interface IEmployeeService : IService<EmployeeInputModel,EmployeeUpdateModel, Employee>
  {
    Task<CustomServiceResultWrapper<Employee>> GetWithBadgeNumberAsync(int employeeNumber);
    Task<CustomServiceResultWrapper<Employee>> GetWithEmailAsync(string email);
    Task<CustomServiceResultWrapper<Employee>> AuthenticateEmployeeAsync(LoginInputModel _employeeLoginModel);
  }
}