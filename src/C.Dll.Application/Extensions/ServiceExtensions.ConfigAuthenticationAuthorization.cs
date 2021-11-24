using ApplicationWebMVC.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection ConfigAuthenticationsAndAuthorizationServices(this IServiceCollection services)
    {
      services.AddAuthentication(options =>
         {
           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;           
         })
              .AddCookieScheme()
              .AddJwtScheme(services)
              .AddAccessTokenScheme()
         ;

      services.AddAuthorization(config =>
      {
        config.DefaultPolicy = new AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .Build();
      });
      return services;
    }
  }
}