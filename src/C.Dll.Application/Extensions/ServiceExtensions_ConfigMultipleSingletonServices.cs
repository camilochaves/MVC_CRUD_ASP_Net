using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
  public static partial class ServiceExtentions
    {
        public static IServiceCollection ConfigMultipleSingletonServices(this IServiceCollection services)
        {         
          
            return services;
        }
    }
}