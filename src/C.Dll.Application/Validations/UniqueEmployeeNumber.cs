using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain.Interfaces;
using HotChocolate.Data.Filters.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Validations
{
    public class UniqueEmployeeNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //Database validation: EmployeeNumber must be unique         
            using var scope = validationContext.CreateScope();
            using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            int? employeeNumber = (int)value;
            var employee = unitOfWork.Employees.Find(x=>x.EmployeeNumber == employeeNumber).FirstOrDefault();
            if (employee is not null)
                return new ValidationResult(
                  "Employee Number already exists in Database! Must be UNIQUE!",
                  new[] { "EmployeeNumber" }
                );
            
            return ValidationResult.Success;
        }

    }
}