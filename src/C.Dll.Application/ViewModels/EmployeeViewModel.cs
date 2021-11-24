using Domain.Entities;

namespace Application.ViewModels
{
  public class EmployeeViewModel : Employee
  {
    public EmployeeViewModel(string firstName,
                             string lastName,
                             string email,
                             string password,
                             int employeeNumber) : base(firstName, lastName, email, password, employeeNumber)
    {
    }
  }

}