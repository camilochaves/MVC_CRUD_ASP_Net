using System;

namespace Domain.Entities
{
  public class ErrorViewModel 
  {
    public string Id { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(Id);
  }
}
