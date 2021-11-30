using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Wrappers.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Middlewares
{
    public class CustomErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate next;
        public CustomErrorHandlingMiddleware(
            RequestDelegate next,
            ILoggerFactory logger)
        {
            this._logger = logger.CreateLogger("GlobalErrorHandling");
            this.next = next;            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {                
                await next(context);
            } catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.Headers.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new CustomAPIResultWrapper<string>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Success = false,
                Message = ex.Message,
                Errors = new List<string>(){$"InnerException Message: {ex.InnerException?.Message}"}
            };

            return context.Response.WriteAsJsonAsync(response);               
        }
    }
}