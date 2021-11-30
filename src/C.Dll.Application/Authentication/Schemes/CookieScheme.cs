using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Authentication
{
  public static partial class StartUpAuthentication
  {
    public static AuthenticationBuilder AddCookieScheme(
        this AuthenticationBuilder authBuilder
        )
    {
      authBuilder.AddCookie
      (CookieAuthenticationDefaults.AuthenticationScheme, op =>
      {
        op.ClaimsIssuer = "Reason Systems";
        op.SlidingExpiration = true;
        op.LoginPath = "/Access/NotLoggedMessage";
        op.AccessDeniedPath = "/Access/AccessDenied";
        
        op.Cookie.Name = "WebApp.Cookie";
        op.Cookie.HttpOnly = false;
        op.Cookie.Expiration = TimeSpan.FromDays(1);
        op.Cookie.MaxAge = TimeSpan.FromDays(30);
        

        op.Events = new CookieAuthenticationEvents()
        {
          OnSigningIn = context =>
                {
                  return Task.CompletedTask;
                },
          OnValidatePrincipal = context =>
                {
                  return Task.CompletedTask;
                },
          OnSignedIn = context =>
                {
                  return Task.CompletedTask;
                },
          OnSigningOut = context =>
                {
                  return Task.CompletedTask;
                }
        };

      });

      return authBuilder;
    }
  }
}