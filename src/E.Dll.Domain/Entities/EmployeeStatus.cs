using System;

namespace Domain.Entities
{
  [Flags]
  public enum EmployeeStatus
  {
    CREATED = 1,
    ACTIVE = 2,
    FIRED = 3
  }
}