using Application.InputModels;
using Application.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationWebMVC.Extensions
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection ConfigCustomValidators(this IServiceCollection services)
    {
      services.AddMvc().AddFluentValidation(
            fv =>
            {
              //fv.RegisterValidatorsFromAssemblyContaining<EmployeeInputModelValidations>();
              fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
              fv.AutomaticValidationEnabled = false;
            });

      //Instead of using fv.RegisterValidatorFromAssemblyContaining<>() as above
      //You can also register validators directly as a service
      services.AddTransient<IValidator<EmployeeUpdateModel>, EmployeeUpdateModelValidations>();
      services.AddTransient<IValidator<EmployeeInputModel>, EmployeeInputModelValidations>();
      
      // services.AddTransient<IValidatorInterceptor, CustomValidatorInterceptor>();

      return services;
    }
  }
}