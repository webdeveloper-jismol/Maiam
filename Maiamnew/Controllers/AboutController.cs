using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    /// <summary>"About Us" menu pages, served at their original WordPress URLs.</summary>
    public class AboutController : Controller
    {
        [Route("objectives")]
        public IActionResult Objectives() => View();

        [Route("about-maiam")]
        public IActionResult AboutMaiam() => View();

        [Route("vision-and-mission")]
        public IActionResult VisionAndMission() => View();

        [Route("testimonials")]
        public IActionResult Testimonials() => View();

        [Route("directors-message")]
        public IActionResult DirectorsMessage() => View();
    }
}
