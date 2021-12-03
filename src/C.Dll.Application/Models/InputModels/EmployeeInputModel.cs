using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Application.Validations;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.InputModels
{
    public class EmployeeInputModel : IValidatableObject
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
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Max lenght is 10, Min Length is 3")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Max lenght is 150, Min Length is 3")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Max lenght is 50, Min Length is 3")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Are you sure this is a valid Email Address ?")]
        [UniqueEmail]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Max lenght is 10, Min Length is 3")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Max lenght is 10, Min Length is 3")]
        [Compare(otherProperty: "Password", ErrorMessage = "Password & confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage = "Number must be between 1 and 1000000")]
        [UniqueEmployeeNumber]
        public int EmployeeNumber { get; set; }

        [StringLength(15, ErrorMessage = "Max size is 15!")]
        [DataType(DataType.Text)]
        public string? Phone { get; set; }

        [StringLength(15, ErrorMessage = "Max size is 15!")]
        [DataType(DataType.Text)]
        public string? MobilePhone { get; set; }

        [Range(0, 9999999999, ErrorMessage = "Value cannot be negative!")]
        [LeaderIdMustExistInDbIfProvided]
        public int? LeaderId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Regex.IsMatch(FirstName, @"^[a-zA-Z]+$"))
                yield return new ValidationResult(
                  "Field must contain only letters",
                  new[] { nameof(FirstName) }
                );

            if (!Regex.IsMatch(LastName, @"^[a-zA-Z]+$"))
                yield return new ValidationResult(
                  "Field must contain only letters",
                  new[] { nameof(LastName) }
                );
        }
    }

}