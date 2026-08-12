using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode:int}")]
        public IActionResult Index(int statusCode)
        {
            ViewData["Title"] = statusCode == 404 ? "Page Not Found" : "Something Went Wrong";
            ViewData["Description"] = "MAIAM - Manappuram Institute of Automotive Skills.";
            ViewBag.StatusCode = statusCode;
            return View();
        }
    }
}
