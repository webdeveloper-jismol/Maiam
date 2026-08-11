using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class FacultiesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Faculties";
            ViewData["Description"] = "Meet MAIAM's expert automotive and welding instructors — decades of hands-on industry experience training the next generation of technicians.";

            var model = new FacultiesViewModel
            {
                Faculty = new List<FacultyMember>
                {
                    new() { Name = "Mr. Rajesh Kumar", Role = "Head of Department - Automotive", Experience = "25+ Years Experience", Qualification = "B.Tech Automobile Engineering" },
                    new() { Name = "Mr. Suresh Nair", Role = "Senior Welding Instructor", Experience = "20+ Years Experience", Qualification = "ITI Welding, AWS Certified" },
                    new() { Name = "Mr. Anil Menon", Role = "Body Repair Specialist", Experience = "18+ Years Experience", Qualification = "Diploma in Auto Body Repair" },
                    new() { Name = "Mr. Vijay Krishnan", Role = "Paint & Finish Expert", Experience = "15+ Years Experience", Qualification = "Certified Paint Technician" },
                }
            };

            return View(model);
        }
    }
}
