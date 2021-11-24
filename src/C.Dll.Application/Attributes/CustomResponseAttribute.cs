using System;
using Application.Wrappers.Response;
using Microsoft.AspNetCore.Mvc;

namespace Application.Wrappers.Attributes
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomResponseAttribute : ProducesResponseTypeAttribute
    {
        public CustomResponseAttribute(int statusCode) : base(typeof(CustomAPIResultWrapper<>), statusCode) { }
    }
}