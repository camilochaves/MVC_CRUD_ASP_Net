using System;
using Infra.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Extensions
{
    public static partial class ServiceExtentions
  {
    public static IServiceCollection ConfigCustomDapperContext(this IServiceCollection services)
    {
      var provider = services.BuildServiceProvider();
      using var scope = provider.CreateScope();
      var secretsHandler = scope.ServiceProvider.GetRequiredService<SecretsHandlerService>();
      var connectionString = secretsHandler.GetFromConfig("MySqlConnection");
      var container = SecretsHandlerService.GetFromEnv("DOTNET_RUNNING_IN_CONTAINER");
      var server = (container=="true") ? "server=DbMySql" : "server=localhost";
      connectionString = server + connectionString;

      
      return services;
    }
  }
}