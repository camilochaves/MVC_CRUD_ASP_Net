using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Application.Wrappers.Response;
using Domain.Entities;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;

namespace Application.GraphQl
{
  [ExtendObjectType(typeof(BaseQueryType))]
  public class EmployeeQueryType
  {
    // [UseProjection]
    // [UseFiltering]
    // [UseSorting]
    public async Task<CustomAPIResultWrapper<IEnumerable<Employee>>> GetEmployees(
      [FromServices] IEmployeeService employeeService)
    {
      var employeeWrapper = await employeeService.GetAllAsync();
      if (!employeeWrapper.Success)
      {
        return new CustomAPIResultWrapper<IEnumerable<Employee>>
        {
          StatusCode = HttpStatusCode.BadRequest,
          Success = false,
          Message = employeeWrapper.Message,
          Errors = employeeWrapper.Errors
        };
      }
      return new CustomAPIResultWrapper<IEnumerable<Employee>>
      {
        StatusCode = HttpStatusCode.OK,
        Success = true,
        Message = employeeWrapper.Message,
        Data = employeeWrapper.Data
      };
    }
    public async Task<CustomAPIResultWrapper<Employee>> EmployeeWithId(
      [FromServices] IEmployeeService context, 
      int id)
    {
      var employeeWrapper = await context.GetWithIdAsync(id);
      if (!employeeWrapper.Success)
      {
        return new CustomAPIResultWrapper<Employee>
        {
          StatusCode = HttpStatusCode.BadRequest,
          Success = false,
          Message = employeeWrapper.Message,
          Errors = employeeWrapper.Errors
        };
      }
      return new CustomAPIResultWrapper<Employee>
      {
        StatusCode = HttpStatusCode.OK,
        Success = true,
        Message = employeeWrapper.Message,
        Data = employeeWrapper.Data
      };
    }
    public async Task<CustomAPIResultWrapper<Employee>> EmployeeWithEmail(
      [FromServices] IEmployeeService context, 
      string email)
    {
      var employeeWrapper = await context.GetWithEmailAsync(email);
      if (!employeeWrapper.Success)
      {
        return new CustomAPIResultWrapper<Employee>
        {
          StatusCode = HttpStatusCode.BadRequest,
          Success = false,
          Message = employeeWrapper.Message,
          Errors = employeeWrapper.Errors
        };
      }
      return new CustomAPIResultWrapper<Employee>
      {
        StatusCode = HttpStatusCode.OK,
        Success = true,
        Message = employeeWrapper.Message,
        Data = employeeWrapper.Data
      };

    } 

  }
}