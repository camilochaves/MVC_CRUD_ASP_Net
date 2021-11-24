using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.InputModels;
using Application.ViewModels;
using Domain.Entities;

namespace Tests
{
  public partial class IntegrationTestsDLLApiJson
  {
    private async Task<HttpResponseMessage> AddEmployee(EmployeeInputModel employeeInputModel)
    {
      var result = await _client.PostAsJsonAsync($"{_factory?.Server.BaseAddress.AbsoluteUri}Employee/AsyncAdd", employeeInputModel);
      var content = await result.Content.ReadAsStringAsync();
      return result;
    }
    private async Task<Employee?> GetEmployeeWithEmail(string email, string access_token)
    {
      HttpRequestMessage requestMessage = new HttpRequestMessage();
      requestMessage.Headers.Add("Access-Token", access_token);
      requestMessage.Method = HttpMethod.Get;
      requestMessage.RequestUri = new Uri($"{_factory?.Server.BaseAddress.AbsoluteUri}Employee/AsyncGetByEmail?email={email}");
      var result = await _client.SendAsync(requestMessage);
      Employee? employee = null;
      APIResultViewModel<Employee>? apiResultWrapper = null;
      if (result.IsSuccessStatusCode)
      {
        apiResultWrapper = await result.Content.ReadFromJsonAsync<APIResultViewModel<Employee>>();
      }
      return apiResultWrapper?.Data;
    }
    private async Task<Employee?> GetEmployeeWithId(int id, string access_token)
    {
      HttpRequestMessage requestMessage = new HttpRequestMessage();
      requestMessage.Headers.Add("Access-Token", access_token);
      requestMessage.Method = HttpMethod.Get;
      requestMessage.RequestUri = new Uri($"{_factory?.Server.BaseAddress.AbsoluteUri}Employee/AsyncGetById?id={id}");
      var result = await this._client.SendAsync(requestMessage);
      Employee? employee = null;
      APIResultViewModel<Employee>? apiResultWrapper = null;
      if (result.IsSuccessStatusCode)
      {
        apiResultWrapper = await result.Content.ReadFromJsonAsync<APIResultViewModel<Employee>>();
      }
      return apiResultWrapper?.Data;
    }
    private async Task<string> LoginAndReturnAccessToken(string email, string password)
    {
      var id_token = await LoginAndReturnIdToken(email, password); //JWT ID TOKEN
      HttpRequestMessage requestMessage = new();
      requestMessage.Headers.Add("Authorization", "Bearer " + id_token);
      requestMessage.Method = HttpMethod.Get;
      requestMessage.RequestUri = new Uri($"{_factory?.Server.BaseAddress.AbsoluteUri}Access/RequestAccessToken");
      var result = await _client.SendAsync(requestMessage);
      APIResultViewModel<string>? content = null;
      if (result.IsSuccessStatusCode)
        content = await result.Content.ReadFromJsonAsync<APIResultViewModel<string>>();
      return content?.Data as String ?? ""; 
    }

    private async Task<string> LoginAndReturnIdToken(string email, string password)
    {
      var user = new LoginInputModel(email, password);
      var result = await _client.PostAsJsonAsync($"{_factory?.Server.BaseAddress.AbsoluteUri}Access/LoginAndReturnToken", user);
      APIResultViewModel<string>? content = null;
      if (result.IsSuccessStatusCode)
        content = await result.Content.ReadFromJsonAsync<APIResultViewModel<string>>();
      var id_token = content?.Data as String ?? "";
      return id_token;
    }

    private static HttpRequestMessage AssembleRequesMessageWithAccessTokenHeader(string access_token, Uri uri, HttpMethod method)
    {
      HttpRequestMessage request = new HttpRequestMessage(method, uri);
      request.Headers.Add("Access-Token", access_token);
      return request;
    }

  }
}