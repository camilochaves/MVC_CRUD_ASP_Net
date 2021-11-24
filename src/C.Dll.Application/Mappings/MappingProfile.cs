using Application.InputModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<EmployeeInputModel, Employee>();
      CreateMap<EmployeeUpdateModel, Employee>();
    }
  }
}