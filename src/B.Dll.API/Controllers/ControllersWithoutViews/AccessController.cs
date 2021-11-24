
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Application.InputModels;
using Application.Services.Interfaces;
using Application.Wrappers.Attributes;
using ApplicationWebMVC.Controllers.Shared;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReasonSystems.DLL.SwissKnife;

namespace ApplicationWebMVC.Controllers
{

  [Route("[controller]")]
  public class AccessController : ApiControllerBase
  {
    private readonly SecretsHandlerService _secretsHandler;
    private readonly IEmployeeService _employeeService;
    private readonly ITokenService<LoginInputModel> _tokenService;

    public AccessController(
            SecretsHandlerService secretsHandler,
            IEmployeeService employeeService,
            ITokenService<LoginInputModel> tokenService
        )
    {
      this._secretsHandler = secretsHandler;
      this._employeeService = employeeService;
      this._tokenService = tokenService;
    }

    [HttpGet]
    [Route("[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public static IActionResult NotLoggedMessage() => ResponseOk("You are not logged!");
    [HttpGet]
    [Route("[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public static IActionResult AccessDenied() => ResponseUnauthorized("Access Denied! UnAuthorized!");


    [HttpPost]
    [Route("[action]")]
    [CustomResponse((int)HttpStatusCode.OK)]
    [CustomResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LoginAndReturnToken([FromBody] LoginInputModel loginInputModel)
    {
      var serviceResult = await _tokenService.LoginAndReturnIdToken(loginInputModel);
      if (!serviceResult.Success) return ResponseBadRequest(serviceResult.Errors);
      var id_token = serviceResult.Data;
      return ResponseOk(id_token);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [CustomResponse((int)HttpStatusCode.OK)]
    [Route("[action]")]
    public async Task<IActionResult> RequestAccessToken()
    {
      var result = await _tokenService.ExchangeIdTokenForAccessToken();
      if (!result.Success) return ResponseBadRequest(result.Errors);
      return ResponseOk(result.Data);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Access-Token-Scheme")]
    [CustomResponse((int)HttpStatusCode.OK)]
    [Route("[action]")]
    public IActionResult GetLoggedUserEmailWithAccessToken()
    {
      var loggedUserEmail = User.Identity.Name;
      return ResponseOk(loggedUserEmail);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [CustomResponse((int)HttpStatusCode.OK)]
    [Route("[action]")]
    public IActionResult GetLoggedUserClaimsWithIdToken()
    {
      return ResponseOk(User.Claims.FirstOrDefault());
    }


  }
}
