using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    public class GalleryController : Controller
    {
        [Route("gallery")]
        public IActionResult Gallery() => View();
    }
}
