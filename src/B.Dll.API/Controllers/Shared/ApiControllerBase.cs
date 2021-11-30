using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Application.Wrappers.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Application.Controllers.Shared
{
  [ApiController]
  public abstract class ApiControllerBase: ControllerBase 
  {
    protected static IActionResult ResponseOk(object data = null) =>
        Response(HttpStatusCode.OK, data: data);

    protected static IActionResult ResponseCreated(object data = null) =>
        Response(HttpStatusCode.Created, data: data);

    protected static IActionResult ResponseNoContent() =>
        Response(HttpStatusCode.NoContent);

    protected static IActionResult ResponseNotModified() =>
        Response(HttpStatusCode.NotModified);

    protected static IActionResult ResponseBadRequest(string errorMessage) =>
        Response(HttpStatusCode.BadRequest, errors: new List<string>(){errorMessage});

    protected static IActionResult ResponseBadRequest(IEnumerable<string> errorMessages) =>
       Response(HttpStatusCode.BadRequest, errorMessages: errorMessages);

    protected static IActionResult ResponseNotFound(string errorMessage = "Resource Not Found!") =>
        Response(HttpStatusCode.NotFound, errors: new List<string>(){errorMessage});

    protected static IActionResult ResponseUnauthorized(string errorMessage = "Permission Denied!") =>
        Response(HttpStatusCode.Unauthorized, null, errors: new List<string>(){errorMessage});


    protected static new JsonResult Response(HttpStatusCode statusCode, object result) =>
        Response(statusCode, data: result);

    protected static new JsonResult Response(HttpStatusCode statusCode, IEnumerable<string> errorMessages) =>
        Response(statusCode, errors: errorMessages);

    protected static new JsonResult Response(HttpStatusCode statusCode, object data = null, IEnumerable<string> errors=null)
    {
      CustomAPIResultWrapper<object> result;
      if (errors.IsNullOrEmpty())
      {
        var success = new HttpResponseMessage(statusCode).IsSuccessStatusCode;

        if (data != null)
        {
          result = new CustomAPIResultWrapper<object>() { StatusCode = statusCode, Success = success, Data = data};
        }
        else
        {
          result = new CustomAPIResultWrapper<object>() { StatusCode = statusCode, Success = success };
        }
      }
      else
      {
        result = new CustomAPIResultWrapper<object>() { StatusCode = statusCode, Errors = errors };
      }
      return new JsonResult(result) { StatusCode = (int)result.StatusCode };
    }

  }
}