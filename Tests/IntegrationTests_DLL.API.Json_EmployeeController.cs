using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.InputModels;
using Application.ViewModels;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{

  public partial class IntegrationTestsDLLApiJson
  {

    //Required Fields: Email, Password, FirstName, LastName, EmployeeNumber
    //Optional Fields: Phone, Mobile, Leader
    [DataTestMethod]
    [DataRow("darthvader@deathstar.com", "123", "125", "Anakyn", "Skywalker", 12345)] //Passwords do not match
    [DataRow("darthvader@deathstar.com", "12", "12", "Anakyn", "Skywalker", 12345)] //Password less than 3 will fail
    [DataRow("darthvader@deathstar.com", "1234567891011", "1234567891011", "Anakyn", "Skywalker", 12345)] //Password bigger than 10 will fail
    [DataRow("darthvader.deathstar.com", "123", "123", "Anakyn", "Skywalker", 12345)] //Invalid email will fail
    [DataRow("darthvader.deathstar.com", "123", "123", "Anakyn", "Skywalker", 13456)] //Employee Number already exists! Test will fail.
    [DataRow("darthvader@deathstar.com", "123", "123", "A", "Skywalker", 12345)] //FirstName less than 3 will fail
    [DataRow("darthvader@deathstar.com", "123", "123", "The", "S", 12345)] //LastName less than 3 will fail
    [DataRow("chaves.camilo@gmail.com", "123", "123", "Camilo", "Chaves", 1)] //Email already exists will fail
    public async Task AsyncAdd_AddEmployeesWhereConstraintsFails_MustNotAdd(
        string email, string password, string confirmPassword, string firstName, string lastName, int employeeNumber
    )
    {
      //Arrange
      await AddEmployee(testEmployee);
      EmployeeInputModel employee = new
        (
            firstName: firstName,
            lastName: lastName,
            email: email,
            password: password,
            confirmPassword: confirmPassword,
            employeeNumber: employeeNumber
        );

      //Act
      var result = await AddEmployee(employee);

      //Asserts
      Assert.IsFalse(result.IsSuccessStatusCode);
    }

    //Required Fields: Email, Password, FirstName, LastName, EmployeeNumber
    //Optional Fields: Phone, Mobile, Leader
    [DataTestMethod]
    [DataRow("", "123", "123", "Camilo", "Chaves", 234543)] //Missing Email will fail
    [DataRow("chaves.camilo@gmail.com", "", "", "Camilo", "Chaves", 234543)] //Missing password will fail
    [DataRow("chaves.camilo@gmail.com", "123", "123", "", "Chaves", 234543)] //Missing FirstName will fail
    [DataRow("chaves.camilo@gmail.com", "123", "123", "Camilo", "", 234543)] //Missing LastName will fail
    [DataRow("chaves.camilo@gmail.com", "123", "123", "Camilo", "Chaves", null)] //Missing Employee Number will fail
    [DataRow("chaves.camilo@gmail.com", "123", "123", "Camilo", "Chaves", 0)] //Employee Number 0 will fail
    [DataRow("chaves.camilo@gmail.com", "123", "123", "Camilo", "Chaves", -1)] //Employee Number less than 0 will fail
    public async Task AsyncAdd_EmployeesMissingRequiredFields_MustNotAdd(
        string email, string password, string confirmPassword, string firstName, string lastName, int? employeeNumber
    )
    {
      //Arrange
      EmployeeInputModel employee = new
        (
            firstName: firstName,
            lastName: lastName,
            email: email,
            password: password,
            confirmPassword: confirmPassword,
            employeeNumber: employeeNumber ?? 0
        );

      //Act
      var result = await AddEmployee(employee);
      //Assert
      Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [TestMethod]
    public async Task GetAllEmployees_AfterAdding1LeaderAnd1Employee_MustReturnListOfEmployees()
    {
      //Arrange : Preparing to Add 2 employees, where First Added is a leader
      var leader = testEmployee;
      EmployeeInputModel employee = new
       (
           firstName: "John",
           lastName: "Rambo",
           email: "johnRambo@gmail.com",
           password: "123",
           confirmPassword: "123",
           employeeNumber: 2
       );
      //Employee above still needs to have LeaderId property. let[s Add a Leader and retrieve it back from Db to 
      //get his ID

      await AddEmployee(leader);
      var access_token = await LoginAndReturnAccessToken(leader.Email, leader.Password);
      var leaderEmployee = await GetEmployeeWithEmail(leader.Email, access_token);

      //After Leader is retrieved, it´s ID is assigned to the Employee´s LeaderId property
      employee.LeaderId = leaderEmployee?.Id;

      await AddEmployee(employee);

      //Act
      var uri = new Uri($"{_factory?.Server.BaseAddress.AbsoluteUri}Employee/AsyncGetAll");
      access_token = await LoginAndReturnAccessToken(employee.Email, employee.Password);
      var requestMessage2 = AssembleRequesMessageWithAccessTokenHeader(access_token,
                                                                       uri,
                                                                       HttpMethod.Get);

      var result = await _client.SendAsync(requestMessage2);
      APIResultViewModel<IEnumerable<Employee>>? apiResultWrapper = null;
      if (result.IsSuccessStatusCode)
      {
        apiResultWrapper = await result.Content.ReadFromJsonAsync<APIResultViewModel<IEnumerable<Employee>>>();
      }
      var employees = apiResultWrapper?.Data;

      //Assert
      Assert.IsTrue(employees?.Any<Employee>(x => x.FirstName == "Camilo"));
      Assert.IsTrue(employees?.Any<Employee>(x => x.FirstName == "John"));
    }

    [DataTestMethod]
    [DataRow(1, 1)]
    [DataRow(344, 2)] //Must return NOT FOUND if ID is not in Database!
    public async Task GetById_AddAnEmployeeToDb_MustReturnEmployeeIfRightIdIsUsed(int id, int pass)
    {
      //Arrange
      var employee = testEmployee;
      await AddEmployee(employee);
      var access_token = await LoginAndReturnAccessToken(employee.Email, employee.Password);

      //Act
      var employeeFromDb = await GetEmployeeWithId(id, access_token);

      //Assert
      switch (pass)
      {
        case 1: Assert.IsTrue(employeeFromDb?.Id == id); break;
        case 2: Assert.IsTrue(employeeFromDb is null); break;
      }

    }

    [DataTestMethod]
    [DataRow("chaves.camilo@gmail.com")] 
    public async Task GetByEmail_AddATestEmployee_MustReturnEmployeeIfRightEmailIsUsed(string email)
    {
      //Arrange
      var employee = testEmployee;
      await AddEmployee(employee);
      var access_token = await LoginAndReturnAccessToken(employee.Email, employee.Password);

      //Act
      var employeeFromDb = await GetEmployeeWithEmail(email, access_token);

      //Assert
      Assert.IsTrue(employeeFromDb?.Email == email); 
    }

    [DataTestMethod]
    [DataRow(1, 1)]
    [DataRow(43453, 2)] //cannot delete ID that doesn't exist!
    public async Task EmployeeController_DeleteById_MustReturnOk(int id, int pass)
    {
      //Arrange
      var employee = testEmployee;
      await AddEmployee(employee);
      var access_token = await LoginAndReturnAccessToken(employee.Email, employee.Password);
      HttpRequestMessage requestMessage = new HttpRequestMessage
      {
        RequestUri = new Uri($"{_factory?.Server.BaseAddress.AbsoluteUri}Employee/AsyncDeleteById?id={id}"),
        Method = HttpMethod.Get
      };
      requestMessage.Headers.Add("Access-Token", access_token);

      //Act      
      var result = await _client.SendAsync(requestMessage);

      //Assert
      switch (pass)
      {
        case 1: Assert.IsTrue(result.IsSuccessStatusCode); break;
        case 2: Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.NotFound); break;
      }

    }

    //Optional properties that can be updated
    //LastName, Email, Password, Confirm password, Phone, Mobile, Leader ID 
    [DataTestMethod]
    [DataRow(1, "Mr", null, null, null, null, null, null)] //Will fail if Lastname is less than 3 chars
    [DataRow(2, null, "darthvader.gmail.com", null, null, null, null, null)] //Will fail if email is invalid
    [DataRow(3, null, null, "123", "124", null, null, null)] //Will fail if password and confirm password do not match
    [DataRow(6, null, null, null, null, null, null, 3244)] //Will fail if LeaderId does not exist in Database
    public async Task Update_WithFailingConstraints_MustReturnFalse(
      int pass,
      string? lastName,
      string? email,
      string? password,
      string? confirmPassword,
      string? phone,
      string? mobile,
      int? leaderId)
    {
      //Arrange
      var initialEmployee = testEmployee;
      await AddEmployee(initialEmployee);
      var access_token = await LoginAndReturnAccessToken(initialEmployee.Email, initialEmployee.Password);

      //The first user on DB will always have ID = 1, this will be the target to be updated
      var uri = new Uri($"{_factory?.Server.BaseAddress}Employee/AsyncUpdate?id=1");
      var requestMessage = AssembleRequesMessageWithAccessTokenHeader(access_token,
                                                                      uri,
                                                                      HttpMethod.Post);

      EmployeeUpdateModel newEmployeeData = new()
      {
        LastName = lastName,
        Email = email,
        Password = password,
        ConfirmPassword = confirmPassword,
        Phone = phone,
        MobilePhone = mobile,
        LeaderId = leaderId
      };

      //Act
      requestMessage.Content = JsonContent.Create(newEmployeeData);
      requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      var result = await _client.SendAsync(requestMessage);

      Assert.IsFalse(result.IsSuccessStatusCode);
    }

    //Optional properties that can be updated
    //LastName, Email, Password, Confirm password, Phone, Mobile, Leader ID 
    [TestMethod]
    [DataRow(1, "Anakyn", null, null, null, null, null, null)] //update valid lastname
    [DataRow(2, null, "darthvader@gmail.com", null, null, null, null, null)] //update valid email
    [DataRow(3, null, null, "abc", "abc", null, null, null)] //update valid password
    [DataRow(4, null, null, null, null, "(55)123456789", null, null)] //update valid phone
    [DataRow(5, null, null, null, null, null, "(55)123456789", null)] //update valid mobile
    public async Task Update_PropertiesWithValidConstraints_MustReturnTrue(
      int pass,
      string? lastName,
      string? email,
      string? password,
      string? confirmPassword,
      string? phone,
      string? mobile,
      int? leaderId)
    {
      //Arrange
      EmployeeUpdateModel newEmployeeData = new()
      {
        LastName = lastName,
        Email = email,
        Password = password,
        ConfirmPassword = confirmPassword,
        Phone = phone,
        MobilePhone = mobile,
        LeaderId = leaderId
      };

      var initialEmployee = testEmployee;
      await AddEmployee(initialEmployee);
      var access_token = await LoginAndReturnAccessToken(initialEmployee.Email, initialEmployee.Password);

      var uri = new Uri($"{_factory?.Server.BaseAddress}Employee/AsyncUpdate?id=1");
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
      request.Headers.Add("Access-Token", access_token);

      //Act
      request.Content = JsonContent.Create(newEmployeeData);
      request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      HttpResponseMessage result = await _client.SendAsync(request);
      string? context = await result.Content.ReadAsStringAsync();

      Assert.IsTrue(result.IsSuccessStatusCode);
    }
  }
}