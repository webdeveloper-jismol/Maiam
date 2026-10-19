using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    public class FacilitiesController : Controller
    {
        [Route("our-facilities")]
        public IActionResult OurFacilities() => View();
    }
}
