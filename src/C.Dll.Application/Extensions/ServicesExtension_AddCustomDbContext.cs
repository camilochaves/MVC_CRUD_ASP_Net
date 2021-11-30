using System;
using Infra.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Extensions
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services)
    {
      var provider = services.BuildServiceProvider();
      using var scope = provider.CreateScope();
      var secretsHandler = scope.ServiceProvider.GetRequiredService<SecretsHandlerService>();
      var connectionString = secretsHandler.GetFromConfig("MySqlConnection");

      //server is : server=localhost when running in windows
      //            server=DbMySql when running in container
      var server = SecretsHandlerService.GetFromEnv("ServerStringConnection");
      connectionString = server + connectionString;

      services.AddPooledDbContextFactory<ApplicationContext>(options =>
      {
        //options.UseInMemoryDatabase("Test.db");
        options.UseMySql(connectionString,
           ServerVersion.AutoDetect(connectionString),
           builder => 
           {
             builder.EnableRetryOnFailure(5);
             builder.CommandTimeout(10);
             builder.MigrationsAssembly("CleanMVC");
           }
        ).LogTo(Console.WriteLine, new[]{DbLoggerCategory.Database.Command.Name});
      });

      services.AddScoped(implementationFactory: sp => sp
            .GetRequiredService<IDbContextFactory<ApplicationContext>>()
            .CreateDbContext());

      return services;
    }
  }
}