using System.ComponentModel.DataAnnotations;

namespace Application.InputModels
{
  public class EmployeeInputModel
  {
    public EmployeeInputModel(
      string firstName,
      string lastName,
      string email,
      string password,
      string confirmPassword,
      int employeeNumber)
    {
      this.FirstName = firstName;
      this.LastName = lastName;
      this.Email = email;
      this.Password = password;
      this.ConfirmPassword = confirmPassword;
      this.EmployeeNumber = employeeNumber;
    }

    [Required]
    [StringLength(10, MinimumLength = 3)]
    [DataType(DataType.Text)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(150, MinimumLength = 3)]
    [DataType(DataType.Text)]
    public string LastName { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [StringLength(10, MinimumLength = 3)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [StringLength(10, MinimumLength = 3)]
    [Compare(otherProperty: "Password", ErrorMessage = "Password & confirm password does not match")]
    public string ConfirmPassword { get; set; }
    [Required]
    public int EmployeeNumber { get; set; }
    [StringLength(15)]
    [DataType(DataType.Text)]
    public string? Phone { get; set; }
    [StringLength(15)]
    [DataType(DataType.Text)]
    public string? MobilePhone { get; set; }
    public int? LeaderId { get; set; }

  }

}