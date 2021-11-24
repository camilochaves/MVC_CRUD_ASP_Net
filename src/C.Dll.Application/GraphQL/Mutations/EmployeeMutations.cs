using System.Threading.Tasks;
using Application.InputModels;
using Application.Services.Interfaces;
using Application.Wrappers.Response;
using Domain.Entities;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.GraphQl
{
   [ExtendObjectType(typeof(BaseMutationType))]
  public class EmployeeMutations
  {

    public async Task<CustomServiceResultWrapper<Employee>> AddEmployee(
      [FromServices] ITopicEventSender sender,
      [FromServices] IEmployeeService context,
      [FromServices] IHttpContextAccessor httpCtx,
      EmployeeInputModel input)
    {
      var result = context.AddAsync(input).Result;
      if (result.Success)
      {
        Employee employee = result.Data;
        await sender.SendAsync(nameof(EmployeeSubscriptionType.EmployeeAdded), employee);
      }
      return result;
    }

    public async Task<CustomServiceResultWrapper<Employee>> RemoveEmployeeWithId(
      [FromServices] IEmployeeService context,
      [FromServices] ITopicEventSender sender,
      int id)
    {
      var result = context.RemoveWithIdAsync(id).Result;
      if (result.Success)
      {
        Employee employee = result.Data;
        await sender.SendAsync(nameof(EmployeeSubscriptionType.EmployeeRemoved), employee);
      }
      return result;
    }

    public async Task<CustomServiceResultWrapper<Employee>> UpdateEmployeeWithId(
      [FromServices] IEmployeeService context,
      [FromServices] ITopicEventSender sender,
      int id,
      EmployeeUpdateModel updateModel)
    {
      var result = context.UpdateAsync(id, updateModel).Result;
      if (result.Success)
      {
        Employee employee = result.Data;
        await sender.SendAsync(nameof(EmployeeSubscriptionType.EmployeeUpdated), employee);
      }
      return result;
    }

  }
}