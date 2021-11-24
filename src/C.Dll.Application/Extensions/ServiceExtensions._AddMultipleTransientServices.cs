using Microsoft.Extensions.DependencyInjection;

namespace ApplicationWebMVC.Extensions
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection AddMultipleTransientServices(this IServiceCollection services)
    {
      return services;
    }
  }
}