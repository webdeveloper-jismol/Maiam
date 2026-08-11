using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult AutoBodyRepair()
        {
            ViewData["Title"] = "Automotive Body Repair Technician";
            ViewData["Description"] = "NCVET, ASDC, NSDC certified Automotive Body Repair Technician course at MAIAM — 1 year, hands-on workshop training with 100% placement assistance.";

            var model = new CourseDetailViewModel
            {
                CertificationTag = "NCVET, ASDC, NSDC CERTIFICATION",
                Title = "Automotive Body Repair Technician",
                Description = "A comprehensive course covering all aspects of automotive body repair including dent repair, painting, panel replacement, and structural repairs.",
                Duration = "1 Year",
                Level = "Level 4",
                Certification = "NCVET / ASDC",
                KeyHighlights = new List<string>
                {
                    "Central Government-approved curriculum",
                    "ASDC-accredited training modules",
                    "Hands-on workshop practice",
                    "Industry-standard tools & equipment",
                    "Internship at leading dealerships",
                    "100% placement assistance",
                },
                CurriculumModules = new List<string>
                {
                    "Vehicle construction and materials",
                    "Panel repair and replacement techniques",
                    "Dent removal and metal finishing",
                    "Automotive painting and refinishing",
                    "Welding and fabrication basics",
                    "Plastic repair and bumper work",
                    "Frame alignment and structural repair",
                    "Surface preparation and masking",
                },
                ThirdTabLabel = "Career Path",
                CareerPaths = new List<CareerPathItem>
                {
                    new() { Title = "Body Repair Technician", Description = "Work at authorized service centers" },
                    new() { Title = "Paint Specialist", Description = "Specialize in automotive refinishing" },
                    new() { Title = "Workshop Supervisor", Description = "Manage repair teams at dealerships" },
                    new() { Title = "Insurance Assessor", Description = "Evaluate vehicle damage for insurers" },
                }
            };

            return View("CourseDetail", model);
        }

        public IActionResult AdvancedWelding()
        {
            ViewData["Title"] = "Advanced Welding Technician";
            ViewData["Description"] = "NSDC certified Advanced Welding Technician course at MAIAM — TIG, MIG/MAG, ARC and pipe welding, regular and weekend batches available.";

            var model = new CourseDetailViewModel
            {
                CertificationTag = "NSDC CERTIFICATION",
                Title = "Advanced Welding Technician",
                Description = "Master TIG, MIG/MAG, and ARC welding techniques including specialized pipe welding. Available in regular and weekend batches.",
                Duration = "6-12 Months",
                Level = "Advanced",
                Certification = "NSDC",
                KeyHighlights = new List<string>
                {
                    "TIG Welding specialization",
                    "MIG/MAG Welding training",
                    "ARC Welding fundamentals",
                    "Pipe Welding techniques",
                    "Regular & Weekend batches",
                    "NSDC certified curriculum",
                },
                CurriculumModules = new List<string>
                {
                    "TIG Welding fundamentals and advanced techniques",
                    "MIG/MAG Welding processes and settings",
                    "ARC Welding (SMAW) for structural work",
                    "Pipe Welding positions and methods",
                    "Welding safety and PPE protocols",
                    "Welding inspection and quality control",
                    "Blueprint reading for welders",
                    "Metal preparation and joint design",
                },
                ThirdTabLabel = "Batches",
                Batches = new List<CourseBatch>
                {
                    new() { Name = "Regular Batch", Duration = "6 Months", Schedule = "Mon-Sat, 9 AM - 5 PM", Mode = "Full Time" },
                    new() { Name = "Weekend Batch", Duration = "12 Months", Schedule = "Sat-Sun, 9 AM - 5 PM", Mode = "Part Time" },
                    new() { Name = "Crash Course", Duration = "1 Month", Schedule = "Mon-Sat, 9 AM - 5 PM", Mode = "Intensive" },
                }
            };

            return View("CourseDetail", model);
        }
    }
}
