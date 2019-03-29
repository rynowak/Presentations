using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace SomeBugs
{
    public class BadInputFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            return true;
        }

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var assembly = Assembly.Load("BadAssemblyThatCantLoad");
            return Task.FromResult(InputFormatterResult.Success(model: null));
        }
    }
}
