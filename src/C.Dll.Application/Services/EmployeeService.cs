using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.InputModels;
using Application.Services.Interfaces;
using Application.Wrappers.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using ReasonSystems.DLL.SwissKnife;

namespace Application.Services
{
  public class EmployeeService : IEmployeeService
  {
    private readonly SecretsHandlerService secretsHandler;
    private readonly IMapper mapper;
    private readonly IValidator<EmployeeInputModel> _inputValidator;
    private readonly IValidator<EmployeeUpdateModel> _updateModelValidator;

    public EmployeeService(
          SecretsHandlerService secretsHandler,
          IUnitOfWork unitOfWork,
          IMapper mapper,
          IValidator<EmployeeInputModel> inputModelValidator,
          IValidator<EmployeeUpdateModel> updateModelValidator)
    {
      this.secretsHandler = secretsHandler;
      this.UnitOfWork = unitOfWork;
      this.mapper = mapper;
      this._inputValidator = inputModelValidator;
      this._updateModelValidator = updateModelValidator;
    }

    ~EmployeeService()
    {
      UnitOfWork.Dispose();
    }

    public IUnitOfWork UnitOfWork { get; }

    public async Task<CustomServiceResultWrapper<Employee>> AddAsync(EmployeeInputModel employeeInput)
    {
      if (employeeInput.LeaderId == 0) employeeInput.LeaderId = null;
      var result = _inputValidator.Validate(employeeInput);
      if (!result.IsValid) return new CustomServiceResultWrapper<Employee>()
      {
        Message = "Check List of Errors!",
        Errors = result.Errors.ConvertAll<string>(x => x.PropertyName + ":" + x.ErrorMessage)
      };
      Employee employee = mapper.Map<Employee>(employeeInput);

      //Encrypt Password
      var AES_Key = secretsHandler.GetFromConfig("AES_SecretKey");
      var salt = secretsHandler.GetFromConfig("AES_Salt");
      employee.Password = employee.Password.EncryptAES256(AES_Key, salt);

      await UnitOfWork.Employees.AddAsync(employee);
      UnitOfWork.Complete();
      employee = await UnitOfWork.Employees.GetByEmailAsync(employeeInput.Email);
      return new CustomServiceResultWrapper<Employee>()
      {
        Data = employee
      };
    }
    public async Task<CustomServiceResultWrapper<IEnumerable<Employee>>> GetAllAsync()
    {
      var employees = await UnitOfWork.Employees.GetAllAsync();
      if (!employees.Any()) return new CustomServiceResultWrapper<IEnumerable<Employee>>()
      {
        Success = true,
        Message = "No Employees Exist in the Database!"
      };
      return new CustomServiceResultWrapper<IEnumerable<Employee>>() { Data = employees };
    }

    public async Task<CustomServiceResultWrapper<Employee>> GetWithIdAsync(int id)
    {
      var employee = await UnitOfWork.Employees.GetByIdAsync(id);
      if (employee is null) return new CustomServiceResultWrapper<Employee>()
      {
        Success = false,
        Message = "Error!",
        Errors = new List<string>() { $"Employee with Id {id} not Found!" }
      };
      return new CustomServiceResultWrapper<Employee>() { Data = employee };
    }
    public async Task<CustomServiceResultWrapper<Employee>> RemoveWithIdAsync(int id)
    {
      var employee = await UnitOfWork.Employees.GetByIdAsync(id);
      if (employee is null) return new CustomServiceResultWrapper<Employee>()
      {
        Success = false,
        Message = "Errors!",
        Errors = new List<string>()
        {
          $"Employee with Id {id} Not Found!",
          $"Change Input Parameter Id:{id}"
        }
      };
      UnitOfWork.Employees.Remove(employee);
      UnitOfWork.Complete();
      return new CustomServiceResultWrapper<Employee>()
      {
        Success = true,
        Message = "Employee with Id {id} removed!",
        Data = employee
      };
    }
    public async Task<CustomServiceResultWrapper<Employee>> UpdateAsync(int id, EmployeeUpdateModel employeeUpdate)
    {
      var result = _updateModelValidator.Validate(employeeUpdate);
      if (!result.IsValid) return new CustomServiceResultWrapper<Employee>()
      {
        Success = false,
        Message = "Errors!",
        Errors = result.Errors.ConvertAll<string>(x => x.PropertyName + ":" + x.ErrorMessage)
      };

      Employee employee = await UnitOfWork.Employees.GetByIdAsync(id);
      if (employee is null) return new CustomServiceResultWrapper<Employee>() { Errors = new List<string>() { $"Employee with Id {id} Not Found!" } };
      UnitOfWork.Employees.Update(employee);
      employee.Email = employeeUpdate.Email ?? employee.Email;
      employee.Password = employeeUpdate.Password ?? employee.Password;
      employee.LastName = employeeUpdate.LastName ?? employee.LastName;
      employee.LeaderId = employeeUpdate.LeaderId ?? employee.LeaderId;
      employee.LeaderId = (employeeUpdate.LeaderId == 0) ? null : employee.LeaderId;
      employee.Phone = employeeUpdate.Phone ?? employee.Phone;
      employee.MobilePhone = (employeeUpdate.MobilePhone) ?? employee.MobilePhone;
      UnitOfWork.Complete();
      employee = await UnitOfWork.Employees.GetByIdAsync(id);
      return new CustomServiceResultWrapper<Employee>()
      {
        Success = true,
        Message = "Employee Updated!",
        Data = employee
      };
    }

    public async Task<CustomServiceResultWrapper<Employee>> GetWithBadgeNumberAsync(int employeeNumber)
    {
      var employee = await UnitOfWork.Employees.GetByBadgeNumberAsync(employeeNumber);
      if (employee is null) return new CustomServiceResultWrapper<Employee>()
      {
        Success = false,
        Message = "Error!",
        Errors = new List<string>()
        {
          $"Employee with Badge Number:{employeeNumber} not Found!"
        }
      };
      return new CustomServiceResultWrapper<Employee>() { Data = employee };
    }

    public async Task<CustomServiceResultWrapper<Employee>> GetWithEmailAsync(string email)
    {
      var employee = await UnitOfWork.Employees.GetByEmailAsync(email);
      if (employee is null) return new CustomServiceResultWrapper<Employee>()
      {
        Success = false,
        Message = "Error!",
        Errors = new List<string>()
        {
          $"Employee with Email: {email} not Found!"
        }
      };
      return new CustomServiceResultWrapper<Employee>() { Data = employee };
    }

    public async Task<CustomServiceResultWrapper<Employee>> AuthenticateEmployeeAsync(LoginInputModel _employeeLoginDTO)
    {
      var employee = await UnitOfWork.Employees.GetByEmailAsync(_employeeLoginDTO.Email);
      if (employee is null) return new CustomServiceResultWrapper<Employee>() { Errors = new List<string>() { "Email is Invalid!" } };
      var AES_Key = secretsHandler.GetFromConfig("AES_SecretKey");
      var salt = secretsHandler.GetFromConfig("AES_Salt");
      var hashedPassword = _employeeLoginDTO.Password.EncryptAES256(AES_Key, salt);
      if (hashedPassword != employee.Password) return new CustomServiceResultWrapper<Employee>() { Errors = new List<string>() { "Wrong Password!" } };
      return new CustomServiceResultWrapper<Employee>() { Success = true, Data = employee };
    }

  }
}