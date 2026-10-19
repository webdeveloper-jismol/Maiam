using Microsoft.AspNetCore.Mvc;

namespace Maiamnew.Controllers
{
    /// <summary>Course pages, served at their original WordPress URLs.</summary>
    public class CoursesController : Controller
    {
        // The original site published the Automotive Body Repair Technician course at /courses/.
        [Route("courses")]
        public IActionResult AutomotiveBodyRepair() => View();

        [Route("advanced-welding-technician")]
        public IActionResult AdvancedWelding() => View();

        // Published on the original site but not linked from its menu.
        [Route("two-wheeler-repair-technician-course")]
        public IActionResult TwoWheelerRepair() => View();

        // Published on the original site but not linked from its menu.
        [Route("welding-technician")]
        public IActionResult WeldingTechnician() => View();
    }
}
