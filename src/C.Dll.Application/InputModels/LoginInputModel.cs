using Domain.DTOs;

namespace Application.InputModels
{
  public class LoginInputModel : EmployeeLoginDTO
  {
    public LoginInputModel(string email, string password) : base(email, password)
    {
    }
  }
}