using System.Net;

namespace Application.Wrappers.Response
{
    public class CustomAPIResultWrapper<T>: CustomServiceResultWrapper<T> 
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}