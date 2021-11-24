using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
  public class EmployeeLoginDTO
  {
    public EmployeeLoginDTO(string email, string password)
    {
      this.Email = email;
      this.Password = password;
    }

    [Required]
    public string Email { get; }
    [Required]
    public string Password { get; }
  }
}