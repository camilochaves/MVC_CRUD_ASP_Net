using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain.Interfaces;
using HotChocolate.Data.Filters.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Validations
{
    public class LeaderIdMustExistInDbIfProvidedAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            int leaderId = (int)(value ?? 0);
            if (value is null) return ValidationResult.Success; //LeaderID can be null
            if (leaderId < 0) return new ValidationResult(
                   "LeaderId cannot be negative!",
                   new[] { "LeaderId" }
               );


            using var scope = validationContext.CreateScope();
            using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var employee = unitOfWork.Employees.Find(x=>x.Id==leaderId).FirstOrDefault();
            if (employee is null)
                return new ValidationResult(
                  "LeaderId does not exist! Please enter a valid LeaderId!",
                  new[] { "LeaderId" }
                );

            return ValidationResult.Success;
        }

    }
}