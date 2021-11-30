using System.Collections.Generic;
using System.Net;

namespace Application.ViewModels
{
  public class APIResultViewModel<T>
  {
    public HttpStatusCode StatusCode { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }
    public IEnumerable<string> Errors { get; set; }

  }

}

