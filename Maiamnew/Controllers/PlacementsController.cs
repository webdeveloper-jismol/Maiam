using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    public class PlacementsController : Controller
    {
        [Route("our-placements")]
        public IActionResult OurPlacements() => View();
    }
}
