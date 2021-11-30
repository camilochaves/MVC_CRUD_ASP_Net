using System;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Extensions
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection AddEmailService(
        this IServiceCollection services)
    {
      var provider = services.BuildServiceProvider();
      var scope = provider.CreateScope();
      var secretsHandler = scope.ServiceProvider.GetRequiredService<SecretsHandlerService>();
      services.AddMailKit(config =>
      {
        config.UseMailKit
              (
                  new MailKitOptions()
             {
               Server = secretsHandler.GetFromConfig("Server_IP"),
               Password = secretsHandler.GetFromConfig("Server_Password"),
               SenderEmail = secretsHandler.GetFromConfig("Sender_Email"),
               SenderName = secretsHandler.GetFromConfig("Sender_Name"),
               Port = Convert.ToInt32(secretsHandler.GetFromConfig("Server_Port"))
             }
              );
              //config.UseMailKit(_config.GetSection("Email").Get<MailKitOptions>());
            });

      services.AddScoped<EmailService>();
      return services;
    }
  }
}