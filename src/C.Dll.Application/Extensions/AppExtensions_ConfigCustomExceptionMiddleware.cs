using Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Application.Extensions
{
    public static partial class AppExtensions
    {
        public static void ConfigCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomErrorHandlingMiddleware>();
        }
    }
}