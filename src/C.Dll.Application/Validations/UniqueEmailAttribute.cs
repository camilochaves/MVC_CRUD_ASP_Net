using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Validations
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //Database validation: Email must be unique         
            using var scope = validationContext.CreateScope();
            using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var employee = unitOfWork.Employees.Find(x=>x.Email == value as string).FirstOrDefault();
            if (employee is not null)
                return new ValidationResult(
                  "Email already exists in Database! Must be UNIQUE!",
                  new[] { "Email" }
                );
            
            return ValidationResult.Success;
        }

    }
}