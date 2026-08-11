using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class PlacementsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Placements";
            ViewData["Description"] = "MAIAM placement record — 100% placement rate, 50+ partner dealerships and 200+ students placed with leading automotive brands.";

            var model = new PlacementsViewModel
            {
                Stats = new List<StatItem>
                {
                    new() { Value = 100, Suffix = "%", Label = "Placement Rate" },
                    new() { Value = 50, Suffix = "+", Label = "Partner Dealerships" },
                    new() { Value = 200, Suffix = "+", Label = "Students Placed" },
                    new() { Value = 25, Suffix = "K+", Label = "Avg Starting Salary" },
                },
                PlacedStudents = new List<PlacedStudent>
                {
                    new() { Name = "Aaris Raja.A", Batch = "Batch 2023-24", Company = "PPS Mahindra", Package = "Rs 27,861" },
                    new() { Name = "Akshay K", Batch = "Batch 2023-24", Company = "Toyota Dealership", Package = "Rs 25,000" },
                    new() { Name = "Muhammed Fasil", Batch = "Batch 2023-24", Company = "Maruti Suzuki", Package = "Rs 24,500" },
                    new() { Name = "Ali Ashkar", Batch = "Batch 2022-23", Company = "Hyundai Service", Package = "Rs 26,000" },
                    new() { Name = "Dhanush Babu", Batch = "Batch 2022-23", Company = "Tata Motors", Package = "Rs 23,500" },
                    new() { Name = "Arjun", Batch = "Batch 2022-23", Company = "Honda Cars", Package = "Rs 24,000" },
                },
                Recruiters = new List<string> { "Mahindra", "Toyota", "Maruti Suzuki", "Hyundai", "Tata Motors", "Honda" }
            };

            return View(model);
        }
    }
}
