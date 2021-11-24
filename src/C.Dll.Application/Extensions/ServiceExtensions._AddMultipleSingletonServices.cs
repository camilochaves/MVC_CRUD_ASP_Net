using Microsoft.Extensions.DependencyInjection;

namespace ApplicationWebMVC.Extensions
{
  public static partial class ServiceExtentions
    {
        public static IServiceCollection AddMultipleSingletonServices(this IServiceCollection services)
        {         
          
            return services;
        }
    }
}