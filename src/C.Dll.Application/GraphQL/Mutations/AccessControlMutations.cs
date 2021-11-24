using System.Threading.Tasks;
using Application.InputModels;
using Application.Services.Interfaces;
using Application.Wrappers.Response;
using Domain.Entities;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.AspNetCore.Mvc;

namespace Application.GraphQl
{
  [ExtendObjectType(typeof(BaseMutationType))]
  public class AccessControlMutation
  {
     public async Task<CustomServiceResultWrapper<string>> RequestAccessToken(
      [FromServices] ITokenService<LoginInputModel> _tokenService,
      LoginInputModel input)
    {
      var idTokenWrapped = await _tokenService.LoginAndReturnIdToken(input);
      if(!idTokenWrapped.Success) return idTokenWrapped;
      var id_token = idTokenWrapped.Data;
      return  _tokenService.ExchangeIdTokenForAccessToken(id_token);
    }

    public async Task<CustomServiceResultWrapper<string>> LoginAndReturnIdToken(
      [FromServices] ITokenService<LoginInputModel> _tokenService,
      LoginInputModel input)
    {
      return await _tokenService.LoginAndReturnIdToken(input);
    }

    public CustomServiceResultWrapper<string> ExchangeIdTokenForAccessToken(
      [FromServices] ITokenService<LoginInputModel> _tokenService,
      string idToken)
    {
      return _tokenService.ExchangeIdTokenForAccessToken(idToken);
    }

     public async Task<CustomServiceResultWrapper<string>> ExchangeIdTokenInHeaderForAccessToken(
      [FromServices] ITokenService<LoginInputModel> _tokenService)
    {
      return await _tokenService.ExchangeIdTokenForAccessToken();
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