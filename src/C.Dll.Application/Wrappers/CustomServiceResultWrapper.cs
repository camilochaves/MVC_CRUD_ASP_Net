using System.Collections.Generic;

namespace Application.Wrappers.Response
{
  public class CustomServiceResultWrapper<T>
  {
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
    private T? data;
    public T? Data
    {
      get => data;
      set { data = value; Success = true; }
    }
    private IEnumerable<string>? errors;
    public IEnumerable<string>? Errors
    {
      get => errors;
      set { errors = value; Success = false; }
    }
  }

}