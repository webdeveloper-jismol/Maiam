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

        // The Director's Message page was removed at the owner's request on 13 Nov 2026.
        // /directors-message/ now 301s to /about-maiam/ (see LegacyUrlMiddleware).
    }
}
