using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.InputModels;
using Application.Services.Interfaces;
using Application.Wrappers.Attributes;
using ApplicationWebMVC.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationWebMVC.Controllers
{
  [Route("[controller]")]
  public class EmployeeController : ApiControllerBase
  {
    private readonly IEmployeeService _employeeService;

    public EmployeeController(
        IEmployeeService employeeService
    )
    {
      this._employeeService = employeeService;
    }

    [HttpPost]
    [Route("[action]")]
    [CustomResponse((int)HttpStatusCode.OK)]
    [CustomResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AsyncAdd([FromBody] EmployeeInputModel employeeInput)
    {
      var result = await _employeeService.AddAsync(employeeInput);
      if (!result.Success) return ResponseBadRequest(result.Errors);
      return ResponseOk(result.Data);
    }

    [HttpPost]
    [Route("[action]")]
    [Authorize(AuthenticationSchemes = "Access-Token-Scheme")]
    [CustomResponse((int)HttpStatusCode.OK)]
    [CustomResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AsyncUpdate(int id, [FromBody] EmployeeUpdateModel employeeUpdateModel)
    {
      var result = await _employeeService.UpdateAsync(id, employeeUpdateModel);
      if (!result.Success) return ResponseBadRequest(result.Errors);
      return ResponseOk(result.Data);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Access-Token-Scheme")]
    [Route("[action]")]
    [CustomResponse((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AsyncGetAll()
    {
      var result = await _employeeService.GetAllAsync();
      return ResponseOk(result.Data);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Access-Token-Scheme")]
    [Route("[action]")]
    [CustomResponse((int)HttpStatusCode.OK)]
    [CustomResponse((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AsyncGetById([FromQuery] int id)
    {
      var result = await _employeeService.GetWithIdAsync(id);
      if (!result.Success) return ResponseNotFound($"Employee with Id {id} not Found!");
      return ResponseOk(result.Data);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Access-Token-Scheme")]
    [Route("[action]")]
    [CustomResponse((int)HttpStatusCode.OK)]
    [CustomResponse((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AsyncGetByEmail([FromQuery] string email)
    {
      var result = await _employeeService.GetWithEmailAsync(email);
      if (!result.Success) return ResponseNotFound(result.Errors?.FirstOrDefault());
      return ResponseOk(result.Data);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Access-Token-Scheme")]
    [Route("[action]")]
    [CustomResponse((int)HttpStatusCode.OK)]
    [CustomResponse((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AsyncDeleteById([FromQuery] int id)
    {
      var result = await _employeeService.RemoveWithIdAsync(id);
      if (!result.Success) return ResponseNotFound(result.Errors?.FirstOrDefault());
      return ResponseOk(result.Data);
    }

  }
}
