using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SomeBugs
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JsonExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var policy = context.FindEffectivePolicy<IExceptionSeverity>();
            var severity = policy?.Severity ?? Severity.NawRelaxItsOk;

            var response = new
            {
                message = "An error occurred. Gee we're real sorry about that!",
                severity = severity.ToString(),
            };

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(response)
            {
                StatusCode = 500,
            };
        }
    }
}
