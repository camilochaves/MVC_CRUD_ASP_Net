using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

//Import Nuget package AutoWrapper.Core

namespace Application.Extensions
{
  public static partial class ServiceExtentions
  {
    public static IServiceCollection AutoWrapperConfig(this IServiceCollection services)
    {
      //To disable the automatic model state validation
      services.Configure<ApiBehaviorOptions>(options =>
     {
       options.SuppressModelStateInvalidFilter = true;
     });

    //Add to Startup
     //app.UseApiResponseAndExceptionWrapper();

      return services;
    }
  }
}