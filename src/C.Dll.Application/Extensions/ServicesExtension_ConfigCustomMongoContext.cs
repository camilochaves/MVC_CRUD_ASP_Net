using Microsoft.Extensions.DependencyInjection;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Extensions
{
    public static partial class ServiceExtentions
  {
    public static IServiceCollection ConfigCustomMongoContext(this IServiceCollection services)
    {
      var provider = services.BuildServiceProvider();
      using var scope = provider.CreateScope();
      var secretsHandler = scope.ServiceProvider.GetRequiredService<SecretsHandlerService>();
      var container = SecretsHandlerService.GetFromEnv("DOTNET_RUNNING_IN_CONTAINER");

      //services.AddScoped<MongoContext>();
      
      return services;
    }
  }
}