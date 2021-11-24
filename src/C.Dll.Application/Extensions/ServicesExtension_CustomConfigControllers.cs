using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ApplicationWebMVC.Extensions
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection CustomConfigForControllers(this IServiceCollection services)
    {
      services.AddControllersWithViews()
              .AddNewtonsoftJson(options =>
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
              );

      return services;

    }
  }
}