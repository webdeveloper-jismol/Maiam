using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    public class ContactController : Controller
    {
        [Route("contact-us")]
        public IActionResult Contact() => View();
    }
}
