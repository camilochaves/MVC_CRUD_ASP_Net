using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Application.InputModels;
using Application.Services.Interfaces;
using Application.Wrappers.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Services
{
  public class EmployeeTokenService : ITokenService<LoginInputModel>
  {
    private readonly IEmployeeService _employeeService;
    private readonly SecretsHandlerService _secrets;
    private readonly IHttpContextAccessor _context;

    public EmployeeTokenService(
          IEmployeeService employeeService,
          SecretsHandlerService secretsHandler,
          IHttpContextAccessor context
      )
    {
      this._employeeService = employeeService;
      this._secrets = secretsHandler;
      this._context = context;
    }
    public async Task<CustomServiceResultWrapper<string>> LoginAndReturnIdToken(LoginInputModel input)
    {
      CustomServiceResultWrapper<Employee> result = await _employeeService.AuthenticateEmployeeAsync(input);
      if (!result.Success) return new CustomServiceResultWrapper<string>()
      {
        Success = false,
        Message = "Check Errors!",
        Errors = result.Errors
      };

      var employee = result.Data;
      GenericIdentity user = new(employee?.Email);
      var claims = new List<Claim>()
            {
                new Claim("uid",employee?.Id.ToString()),
                new Claim("badgeNumber",employee?.EmployeeNumber.ToString()),
                new Claim("leaderId", employee?.LeaderId.ToString() ?? "-1")
            };
      user.AddClaims(claims);
      var id_token = TokenTools.CreateIdToken(user, _secrets.GetFromConfig("JWT_SecretKey"));
      return new CustomServiceResultWrapper<string>()
      {
        Success = true,
        Data = id_token
      };
    }

    public CustomServiceResultWrapper<string> ExchangeIdTokenForAccessToken(string idToken)
    {
      try{
      var JWT_SecretKey = _secrets.GetFromConfig("JWT_SecretKey");
      var AES_SecretKey = _secrets.GetFromConfig("AES_SecretKey");
      var AES_Salt = _secrets.GetFromConfig("AES_Salt");
      var result = TokenTools.ValidateIdToken(idToken, JWT_SecretKey);

      if (!result) return new CustomServiceResultWrapper<string>()
      {
        Success = false,
        Message = "Check Errors!",
        Errors = new List<string>() { "Invalid ID Token!" }
      };

      ClaimsPrincipal principal = TokenTools.ExtractPrincipalFromIdToken(
        idToken,
        _secrets.GetFromConfig("JWT_SecretKey")
      );

      _context.HttpContext.User = principal;

      var IP = _context.HttpContext.Request.Host.Host;
      var userAgent = _context.HttpContext.Request.Headers["User-Agent"];

      var access_token = TokenTools.CreateAccessToken(
        principal.Identity.Name,
        AES_SecretKey,
        AES_Salt,
        IP,
        userAgent);
      return new CustomServiceResultWrapper<string>()
      {
        Success = true,
        Data = access_token
      };
      } catch (Exception ex)
      {
        return new CustomServiceResultWrapper<string>()
        {
          Success = false,
          Errors = new List<string>(){ex.Message}
        };
      }
    }

    //Header must contain <key,value> Authorization: "Bearer someIdToken"
    public async Task<CustomServiceResultWrapper<string>> ExchangeIdTokenForAccessToken()
    {
      var authResult = await _context.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
      if (!authResult.Succeeded)
      {
        return new CustomServiceResultWrapper<string>()
        {
          Success = authResult.Succeeded,
          Message = authResult?.Failure?.Message,
          Errors = new List<string>(){
            "Invalid ID Token or ID Token not found!",
            "Authentication with JWT Bearer Failed!",
            authResult?.Failure?.InnerException?.Message
          }
        };
      }
      // var idToken = _context.HttpContext.Request.Headers["Authorization"];      
      var idToken = authResult.Ticket.Properties.GetTokenValue("access_token");
      var IP = _context.HttpContext.Request.Host.Host;
      var userAgent = _context.HttpContext.Request.Headers["User-Agent"];

      var access_token = TokenTools.CreateAccessToken(
        _context.HttpContext.User.Identity.Name,
        _secrets.GetFromConfig("AES_SecretKey"),
        _secrets.GetFromConfig("AES_Salt"),
        IP,
        userAgent);
      return new CustomServiceResultWrapper<string>()
      {
        Success = true,
        Message = idToken,
        Data = access_token
      };
    }
  }
}