using Application.InputModels;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Application.Validations
{
  public class EmployeeInputModelValidations : AbstractValidator<EmployeeInputModel>
  {

    public EmployeeInputModelValidations([FromServices] IUnitOfWork ctx)
    {
      ValidateFirstName();
      ValidateLastName();
      ValidateEmail(ctx);
      ValidatePassword();
      ValidateEmployeeNumber(ctx);
      ValidateLeaderId(ctx);
    }

    protected void ValidateLeaderId(IUnitOfWork _ctx)
    {
      RuleFor(c=>c.LeaderId)
        .GreaterThanOrEqualTo(0).WithMessage("You cannot add a negative Leader Id!")
        .MustAsync( async (leaderId, cancellation) => {
          if(leaderId==0 || leaderId is null) return true;
          var employee = await _ctx.Employees.GetByIdAsync(leaderId??0);
          return (employee is not null);
        })
        .WithMessage("Add a Leader whose Id already exists in Database");
    }
    protected void ValidateFirstName()
    {
      RuleFor(c => c.FirstName)
          .NotEmpty().WithMessage("Please ensure you have entered the Name")
          .Length(3, 10).WithMessage("The Name must have between 3 and 10 characters");
    }

    protected void ValidateLastName()
    {
      RuleFor(c => c.LastName)
          .NotEmpty().WithMessage("Please ensure you have entered the LastName")
          .Length(3, 150).WithMessage("The Last must have between 3 and 150 characters");
    }

    protected void ValidateEmail(IUnitOfWork _ctx)
    {
      RuleFor(c => c.Email)
          .NotEmpty()
          .EmailAddress()
          .Length(3, 50).WithMessage("The Last must have between 3 and 50 characters")
          .Must(email=> _ctx.Employees.Find(y=>y.Email==email).IsNullOrEmpty())
          .WithMessage("Email already exists in Database!");
    }

    protected void ValidateEmployeeNumber(IUnitOfWork _ctx)
    {
      RuleFor(c => c.EmployeeNumber)
          .GreaterThan(0).WithMessage("Employee Number cannot be 0 or <0")
          .Must(x => _ctx.Employees.Find(y=>y.EmployeeNumber==x).IsNullOrEmpty())
          .WithMessage("Employee Number already exists!");
    }

    protected void ValidatePassword()
    {
      RuleFor(c => c.Password)
      .NotEmpty().WithMessage("Password must not be empty!")
      .Length(3, 10).WithMessage("Passwords must have between 3 and 10 characters")
      .Equal(x=>x.ConfirmPassword).WithMessage("Password do not match Confirm Password!");
    }


  }
}