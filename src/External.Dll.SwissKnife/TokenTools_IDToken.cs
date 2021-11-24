using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ReasonSystems.DLL.SwissKnife
{
  public partial class TokenTools
  {
    public static string CreateIdToken(
        ClaimsIdentity user,
        string secretKey,
        string issuer = "Reason Systems",
        SecurityTokenDescriptor tokenDescriptor = null)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var privateTokenKey =
      Encoding.ASCII.GetBytes(secretKey);
      var _tokenDescriptor = tokenDescriptor ?? new SecurityTokenDescriptor()
      {
        Subject = user,
        Expires = DateTime.UtcNow.AddHours(8),
        SigningCredentials = new SigningCredentials(
              new SymmetricSecurityKey(privateTokenKey),
              SecurityAlgorithms.HmacSha256Signature),
        Issuer = issuer,
        IssuedAt = DateTime.UtcNow,
        NotBefore = DateTime.UtcNow
      };
      var token = tokenHandler.CreateToken(_tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public static bool ValidateIdToken
    (
        string token,
        string secretKey,
        TokenValidationParameters tokenValidationParameters = null
    )
    {
      if (tokenValidationParameters is null)
      {
        tokenValidationParameters = new TokenValidationParameters()
        {
          ValidIssuer = "Reason Systems",
          ValidateIssuer = true,
          ValidateAudience = false,
          IssuerSigningKey =
                 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
          ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };
      }

      JwtSecurityTokenHandler validator = new();

      if (validator.CanReadToken(token))
      {
        ClaimsPrincipal principal;
        try
        {
          // This line throws if invalid
          principal = validator.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

          // If we got here then the token is valid                   
          return true;
        }
        catch (Exception)
        {
          throw;
        }
      }

      return false;
    }

    public static ClaimsPrincipal ExtractPrincipalFromIdToken(
        string idToken,
        string secretKey,
        TokenValidationParameters tokenValidationParameters = null)
    {
      if (tokenValidationParameters is null)
      {
        tokenValidationParameters = new TokenValidationParameters()
        {
          ValidIssuer = "Reason Systems",
          ValidateIssuer = true,
          ValidateAudience = false,
          IssuerSigningKey =
                 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
          ClockSkew = TimeSpan.Zero 
        };
      }
      var tokenHandler = new JwtSecurityTokenHandler();
      ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(
          idToken, tokenValidationParameters, out _);
      return claimsPrincipal;
    }

  }
}