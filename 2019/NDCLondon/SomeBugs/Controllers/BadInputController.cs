using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SomeBugs.Models
{
    [ApiController]
    public class BadFormatterController : Controller
    {
        [ExceptionSeverity(Severity.EverythingIsOnFire)]
        [HttpPost("/BadFormatterData")]
        public ActionResult ReadSomeAwesomeData(AwesomeData data)
        {
            if (!ModelState.IsValid)
            {
                return Content("Invalid Yo!");
            }

            return Content("Everything is fine.");
        }

        [HttpPost("/EnumData")]
        public ActionResult ReadAwesomeEnum([FromForm] PetKind pet)
        {
            if (!ModelState.IsValid)
            {
                return Content("Invalid Yo!");
            }

            return 
                pet < PetKind.Dog || pet > PetKind.Fish ? 
                    Content("Everything is fine... Wait, what?") : 
                    Content("Everything is fine.");
        }

        [HttpPost("/ValidateParameterData")]
        public ActionResult ValidateAwesomeParameter([FromForm, Required] string email)
        {
            if (!ModelState.IsValid)
            {
                return Content("Invalid Yo!");
            }

            return 
                email == null ? 
                Content("Everything is fine... Wait, what?") : 
                Content("Everything is fine.");
        }
    }
}
