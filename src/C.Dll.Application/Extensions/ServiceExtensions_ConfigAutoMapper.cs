using System;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
  public static partial class ServiceExtentions
    {
        public static IServiceCollection ConfigAutoMapper(this IServiceCollection services)
        {        
            return services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}