using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int Id { get; set; }

    [BsonRequired]
    public string FirstName { get; set; }

    [BsonRequired]
    public string LastName { get; set; }

    [BsonRequired]
    public string Email { get; set; }

    [BsonRequired]
    public string Password { get; set; }

    [BsonRequired]
    public int EmployeeNumber { get; set; }

    public string? Phone { get; set; }
    public string? MobilePhone { get; set; }
    public int? LeaderId { get; set; }
    public Employee? Leader { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.CREATED;
    }
}