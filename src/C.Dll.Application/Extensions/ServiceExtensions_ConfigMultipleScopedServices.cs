using Application.InputModels;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Infra.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Extensions
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection ConfigMultipleScopedServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddScoped(implementationFactory: provider =>
      {
        return new SecretsHandlerService(configuration);
      });


      services.AddScoped<IEmployeeService, EmployeeService>();
      services.AddScoped<ITokenService<LoginInputModel>, EmployeeTokenService>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped<RedisService>();
      
      return services;
    }
  }
}