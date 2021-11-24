using System.ComponentModel.DataAnnotations;

namespace Application.InputModels
{
  public class EmployeeUpdateModel
  {
    [StringLength(10, MinimumLength = 3)]
    [DataType(DataType.Text)]
    public string? LastName { get; set; }

    [EmailAddress]
    [StringLength(50, MinimumLength = 3)]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [StringLength(10, MinimumLength = 3)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [StringLength(10, MinimumLength = 3)]
    [Compare(otherProperty: "Password", ErrorMessage = "Password & confirm password does not match")]
    public string? ConfirmPassword { get; set; }
    
    [StringLength(15)]
    [DataType(DataType.Text)]
    public string? Phone { get; set; }
    
    [StringLength(15)]
    [DataType(DataType.Text)]
    public string? MobilePhone { get; set; }

    public int? LeaderId { get; set; }
  }

}