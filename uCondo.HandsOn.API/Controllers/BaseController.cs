using Microsoft.AspNetCore.Mvc;
using uCondo.HandsOn.Domain.Validation;

namespace uCondo.HandsOn.API.Controllers
{
    public class BaseController : Controller
	{
        protected static IActionResult BadRequest(ValidationResult result)
        {
            return new BadRequestObjectResult(new
            {
                result.Message
            });
        }
    }
}