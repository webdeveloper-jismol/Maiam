using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class FacilitiesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Facilities";
            ViewData["Description"] = "Explore MAIAM's campus facilities — advanced workshop, welding lab, paint booth, classrooms, hostel and library.";

            var model = new FacilitiesViewModel
            {
                Facilities = new List<IconTextCard>
                {
                    new() { IconKey = "workshop", Title = "Advanced Workshop", Description = "Fully equipped automotive workshop with modern diagnostic tools and repair equipment." },
                    new() { IconKey = "welding", Title = "Welding Lab", Description = "Specialized welding laboratory with TIG, MIG/MAG, and ARC welding stations." },
                    new() { IconKey = "paint", Title = "Paint Booth", Description = "Professional paint spraying booth with proper ventilation and safety equipment." },
                    new() { IconKey = "classroom", Title = "Classrooms", Description = "Air-conditioned classrooms with modern teaching aids and comfortable seating." },
                    new() { IconKey = "hostel", Title = "Hostel Facilities", Description = "Clean and comfortable hostel accommodation for outstation students." },
                    new() { IconKey = "library", Title = "Library", Description = "Well-stocked library with automotive reference books, manuals, and digital resources." },
                },
                Amenities = new List<string>
                {
                    "High-Speed WiFi", "Cafeteria", "Parking", "First Aid",
                    "Computer Lab", "Smart Classrooms", "Tool Store", "Green Campus"
                }
            };

            return View(model);
        }
    }
}
