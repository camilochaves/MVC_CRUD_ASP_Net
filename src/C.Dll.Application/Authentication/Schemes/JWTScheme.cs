using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Authentication
{
  public static partial class StartUpAuthentication
    {
        public static AuthenticationBuilder AddJwtScheme(
            this AuthenticationBuilder authBuilder,
            IServiceCollection services
            )
        {
            var provider = services.BuildServiceProvider();
            var scope = provider.CreateScope();
            var secretsHandler = scope.ServiceProvider.GetRequiredService<SecretsHandlerService>();

            authBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.SaveToken = true;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = "Reason Systems",
                       ValidateIssuer = true,
                       ValidateAudience = false,
                       IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretsHandler.GetFromConfig("JWT_SecretKey"))),
                       ClockSkew = TimeSpan.Zero // remove delay of token when expire
                   };
                   options.Events = new JwtBearerEvents()
                   {
                       OnTokenValidated = context =>                       
                       {
                           context.HttpContext.User = context.Principal;
                           return Task.CompletedTask;
                       },
                       OnMessageReceived = context =>
                       {
                           return Task.CompletedTask;
                       },
                       OnAuthenticationFailed = context =>
                       {
                           return Task.CompletedTask;
                       },
                       OnForbidden = context =>
                       {
                           return Task.CompletedTask;
                       }
                   };
               });
            return authBuilder;
        }
    }
}