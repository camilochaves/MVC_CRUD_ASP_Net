using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Application.Extensions
{
  public static partial class ServiceExtentions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: Bearer 1safsfsdfdfd",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                 // add Access-Token Custom Authentication
                var securityScheme2 = new OpenApiSecurityScheme
                {
                    Name = "Access-Token",
                    Description = "Enter Access-Token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "access-token-scheme", // must be lower case
                    //BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Access-Token",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(securityScheme2.Reference.Id, securityScheme2);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme2, new string[] { }}
                });


                //c.DocumentFilter<CustomSwaggerDocFilter>();
                //c.SchemaFilter<SwaggerIgnoreFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Zup App",
                    Version = "v1",
                    Description = "An ASP.NET CORE API",
                    Contact = new OpenApiContact
                    {
                        Name = "Main ZUP API Website",
                        Url = new System.Uri("https://localhost:5001")
                    }
                });
            });

        }
    }
}