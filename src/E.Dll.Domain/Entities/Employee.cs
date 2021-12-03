using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
  public class Employee 
  {
    public Employee(string firstName,
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
      this.Status = EmployeeStatus.CREATED;
    }
    //[Key]
    public int Id { get; set; }
    //[Required]
    //[StringLength(10,MinimumLength =3)]
    //[DataType(DataType.Text)]
    public string FirstName { get; set; }
    //[Required]
    //[StringLength(150,MinimumLength =3)]
    //[DataType(DataType.Text)]
    public string LastName { get; set; }
    //[Required]
    //[StringLength(50, MinimumLength = 3)]
    //[DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    //[Required]
    //[StringLength(100)]
    //[DataType(DataType.Password)]
    public string Password { get; set; }
    //[Required]
    public int EmployeeNumber { get; set; }
    //[StringLength(15)]
    //[DataType(DataType.Text)]
    public string? Phone { get; set; }
    //[StringLength(15)]
    //[DataType(DataType.Text)]
    public string? MobilePhone { get; set; }
    public int? LeaderId { get; set; }

    //[ForeignKey("LeaderId")]
    public Employee? Leader { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.CREATED;
    }
}