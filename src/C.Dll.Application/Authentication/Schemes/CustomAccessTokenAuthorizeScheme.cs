using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Authentication
{
    public class ValidateAccessTokenSchemeOptions : AuthenticationSchemeOptions
    { 
        public bool SaveAccessToken { get; set; }
        public ValidateAccessTokenSchemeOptions()
        {
            
        }
    }

    public class AccessTokenHandler
        : AuthenticationHandler<ValidateAccessTokenSchemeOptions>
    {
        private readonly SecretsHandlerService _secretsHandler;

        public AccessTokenHandler(
            IOptionsMonitor<ValidateAccessTokenSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            SecretsHandlerService secretsHandler)
            : base(options, logger, encoder, clock)
        {
            this._secretsHandler = secretsHandler;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // validation comes in here
            if (!Request.Headers.ContainsKey("Access-Token"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Access-Token Not Found on Request Headers!"));
            }

            var access_token = Request.Headers["Access-Token"];
            var isValid = TokenTools.IsValidAccessToken(
                access_token,
                Request.Host.Host,
                Request.Headers["User-Agent"].ToString(),
                _secretsHandler.GetFromConfig("AES_SecretKey"),
                _secretsHandler.GetFromConfig("AES_Salt")   
                );

            if (!isValid) Task.FromResult(AuthenticateResult.Fail("Access-Token Not Valid!"));

            var claimsIdentity = new GenericIdentity(TokenTools.DecodeAccessTokenAndSeparateParts(access_token)[1]);

            // generate AuthenticationTicket 
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

}