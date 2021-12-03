using System.Net.Mail;
using Application.InputModels;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Application.Validations
{
  public class EmployeeUpdateModelValidations : AbstractValidator<EmployeeUpdateModel>
  {
    public EmployeeUpdateModelValidations([FromServices] IUnitOfWork ctx)
    {
      ValidateLeaderId(ctx);
      ValidateLastName();
      ValidatePassword();
      ValidateEmail(ctx);
    }

    protected void ValidateLeaderId(IUnitOfWork _ctx)
    {
      RuleFor(c => c.LeaderId)
        .MustAsync(async (leaderId, cancellation) =>
        {
          if (leaderId == 0 || leaderId is null) return true;
          var employee = await _ctx.Employees.GetByIdAsync(leaderId ?? 0);
          return employee is not null;
        })
        .WithMessage("Add a Leader whose Id already exists in Database");
    }

    protected void ValidateLastName()
    {
      RuleFor(c => c.LastName)
          .Must(lastName =>
          {
            if (lastName.IsNullOrEmpty()) return true;
            if (lastName.Length < 3) return false;
            return true;
          })
          .WithMessage("Length must be > 3");
    }

    protected void ValidateEmail(IUnitOfWork _ctx)
    {
      RuleFor(c => c.Email)
         .Must(email =>
         { 
           if (email.IsNullOrEmpty()) return true;
           return MailAddress.TryCreate(email, out _);
         })
         .WithMessage("Invalid Email Format!");

      RuleFor(c => c.Email)
         .Must(email => 
         {
           if(email.IsNullOrEmpty()) return true;
           return _ctx.Employees.Find(y => y.Email == email).IsNullOrEmpty();
         })
         .WithMessage("Email already exists in Database!");
    }

    protected void ValidatePassword()
    {
      RuleFor(c => c.Password)
      .Must(password =>
      {
        if (password.IsNullOrEmpty()) return true;
        if (password?.Length < 3) return false;
        return true;
      }).WithMessage("Password cannot be less than 3 characteres!")
      .Equal(c=>c.ConfirmPassword).WithMessage("Password and ConfirmPassword must match");
    }
  }
}