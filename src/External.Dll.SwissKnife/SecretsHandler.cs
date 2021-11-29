using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ReasonSystems.DLL.SwissKnife
{
    public class SecretsHandlerService
    {
        private readonly IConfiguration configuration;

        public SecretsHandlerService(
            IConfiguration configuration
        )
        {
            this.configuration = configuration;
        }

        public string GetFromConfig(string key)
        {
            var section = configuration.GetSection(key);
            return section.Value;
        }

        public static string GetFromEnv(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }

        public T GetObject<T>(string key) where T : class
        {
            try
            {
                var children = configuration.GetSection(key).GetChildren();
                T result = Activator.CreateInstance<T>();
                PropertyInfo property = null;
                foreach(var section in children)
                {
                    var name = section.Key;
                    property = result.GetType().GetProperty(name);
                    property?.SetValue(result, section.Value);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}