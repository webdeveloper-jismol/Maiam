using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    public class FacultiesController : Controller
    {
        [Route("faculties")]
        public IActionResult Faculties() => View();
    }
}
