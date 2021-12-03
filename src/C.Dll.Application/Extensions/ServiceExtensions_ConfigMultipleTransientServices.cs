using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection ConfigMultipleTransientServices(this IServiceCollection services)
    {
      return services;
    }
  }
}