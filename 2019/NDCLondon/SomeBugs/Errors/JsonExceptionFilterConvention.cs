using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SomeBugs
{
    public class JsonExceptionFilterConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (IsApiController(controller))
            {
                controller.Filters.Add(new JsonExceptionFilterAttribute());
            }
        }

        private static bool IsApiController(ControllerModel controller)
        {
            if (controller.Attributes.OfType<ApiControllerAttribute>().Any())
            {
                return true;
            }

            return controller
                .ControllerType
                .Assembly
                .GetCustomAttributes(inherit: true)
                .OfType<ApiControllerAttribute>()
                .Any();
        }
    }
}
