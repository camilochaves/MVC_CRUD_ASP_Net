using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.DTOs
{
  public class EmployeeRegisterDTO
  {
    public EmployeeRegisterDTO(
               string firstName,
               string lastName,
               string email,
               string password,
               int employeeNumber)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      Password = password;
      EmployeeNumber = employeeNumber;
    }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public int EmployeeNumber { get; set; }
    public string? Phone { get; set; }
    public string? MobilePhone { get; set; }
    public int? LeaderId { get; set; }
    [ForeignKey("LeaderId")]
    public Employee? Leader { get; set; }
  }
}
