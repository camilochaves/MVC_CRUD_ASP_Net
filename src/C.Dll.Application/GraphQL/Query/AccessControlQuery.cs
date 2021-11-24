using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Wrappers.Response;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReasonSystems.DLL.SwissKnife;

namespace Application.GraphQl
{
  [ExtendObjectType(typeof(BaseQueryType))]
  public class AccessControlQueryType
  {
    public async Task<CustomAPIResultWrapper<IEnumerable<string>>> ValidateIdToken(
      [FromServices] IHttpContextAccessor _context)
    {
      AuthenticateResult authResult = await _context.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
      if (!authResult.Succeeded)
      {
        return new CustomAPIResultWrapper<IEnumerable<string>>()
        {
          StatusCode = System.Net.HttpStatusCode.Unauthorized,
          Success = false,
          Message = "Unauthorized",
          Errors = new List<string>() { "Id-Token Invalid or Not Present!" }
        };
      }

      _context.HttpContext.User = authResult.Principal;


      return new CustomAPIResultWrapper<IEnumerable<string>>()
      {
        StatusCode = HttpStatusCode.OK,
        Success = true,
        Message = "User is Authorized",
        Data = new List<string>()
        {
          "Logger User: "+ _context.HttpContext.User.Identity.Name
        }
      };
    }

    public async Task<CustomAPIResultWrapper<IEnumerable<string>>> ValidateAccessToken(
     [FromServices] IHttpContextAccessor _context)
    {
      AuthenticateResult authResult = await _context.HttpContext.AuthenticateAsync("Access-Token-Scheme");
      if (!authResult.Succeeded)
      {
        return new CustomAPIResultWrapper<IEnumerable<string>>()
        {
          StatusCode = HttpStatusCode.Unauthorized,
          Success = false,
          Message = "Authentication Failed!",
          Errors = new List<string>() { "Access-Token Invalid or Not Present!" }
        };
      }

      _context.HttpContext.User = authResult.Principal;

      var access_token = _context.HttpContext.Request.Headers["Access-Token"];
      var result = TokenTools.DecodeAccessTokenAndSeparateParts(access_token);
      var hash = result[0];
      var username = result[1];
      var timeConstraint = result[2];
      return new CustomAPIResultWrapper<IEnumerable<string>>()
      {
        StatusCode = HttpStatusCode.OK,
        Success = true,
        Message = "User is Authorized",
        Data = new List<string>()
        {
          "LoggedUser: "+_context.HttpContext.User.Identity.Name,
          "access-Token hash: "+hash,
          "access-Token username: "+username,
          "access-Token timeConstraint: "+timeConstraint
        }
      };
    }

  }
}