using Maiam.Models;
using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Maiam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            ViewData["Description"] = "MAIAM - Kerala's 1st Central Government-approved, ASDC-accredited automotive body repair and welding skills institute. Explore courses, placements and facilities.";

            var model = new HomeViewModel
            {
                HeroSlides = new List<HeroSlide>
                {
                    new() { ImageUrl = "/images/home/hero/slide-1.jpg", Heading = "Shape Your Automotive Career", Subheading = "Kerala's 1st Central Government-approved, ASDC-accredited institution" },
                    new() { ImageUrl = "/images/home/hero/slide-2.jpg", Heading = "Automotive Body Repair Technician", Subheading = "NCVET, ASDC, NSDC Certification | 100% Placement Support" },
                    new() { ImageUrl = "/images/home/hero/slide-3.jpg", Heading = "Advanced Welding Technician", Subheading = "TIG, MIG/MAG, ARC Welding Including Pipe Welding" },
                    new() { ImageUrl = "/images/home/hero/slide-4.jpg", Heading = "Scholarships Available", Subheading = "10% for BPL students | 5% for APL backward students" },
                    new() { ImageUrl = "/images/home/hero/slide-5.jpg", Heading = "Expert Training & Facilities", Subheading = "25+ Years Experienced Trainers | Fully Mechanized Practical Lab" },
                    new() { ImageUrl = "/images/home/hero/slide-6.png", Heading = "Join MAIAM Today", Subheading = "Register now for the upcoming batch" },
                },
                FeatureCards = new List<FeatureCard>
                {
                    new() { IconKey = "shield", Title = "Kerala's 1st Government Approved", Description = "Central Government-approved, ASDC-accredited institution in Automotive Body Repair Course" },
                    new() { IconKey = "trainer", Title = "25 Years Experienced Trainers", Description = "Learn from industry experts with decades of hands-on experience" },
                    new() { IconKey = "placement", Title = "100% Placement Support", Description = "All India based dealership placements with excellent starting packages" },
                    new() { IconKey = "workshop", Title = "Fully Mechanized Practical Lab", Description = "State-of-the-art workshop with modern equipment and tools" },
                    new() { IconKey = "scholarship", Title = "BPL/APL Scholarship", Description = "10% scholarship for BPL & 5% for APL backward students" },
                },
                Courses = new List<CourseSummary>
                {
                    new()
                    {
                        ImageUrl = "/images/home/hero/slide-2.jpg",
                        Duration = "1 Year",
                        CertificationTag = "NCVET, ASDC, NSDC CERTIFICATION",
                        Title = "Automotive Body Repair Technician",
                        Description = "A comprehensive course covering all aspects of automotive body repair including dent repair, painting, panel replacement, and structural repairs.",
                        Highlights = new List<string> { "Central Government-approved curriculum", "ASDC-accredited training modules", "Hands-on workshop practice", "Industry-standard tools & equipment" },
                        Controller = "Courses",
                        Action = "AutoBodyRepair"
                    },
                    new()
                    {
                        ImageUrl = "/images/home/hero/slide-3.jpg",
                        Duration = "6 Months - 1 Year",
                        CertificationTag = "NSDC CERTIFICATION",
                        Title = "Advanced Welding Technician",
                        Description = "Master TIG, MIG/MAG, and ARC welding techniques including specialized pipe welding. Available in regular and weekend batches.",
                        Highlights = new List<string> { "TIG Welding specialization", "MIG/MAG Welding training", "ARC Welding fundamentals", "Pipe Welding techniques" },
                        Controller = "Courses",
                        Action = "AdvancedWelding"
                    }
                },
                Stats = new List<StatItem>
                {
                    new() { Value = 500, Suffix = "+", Label = "Students Trained" },
                    new() { Value = 100, Suffix = "%", Label = "Placement Rate" },
                    new() { Value = 25, Suffix = "+", Label = "Years Experience" },
                    new() { Value = 2, Suffix = "", Label = "Certified Programs" },
                },
                Testimonials = new List<TestimonialItem>
                {
                    new() { Quote = "Hi, I am Aris Raja from a hilly village in Idukki and I came to know about the automotive body repair technician course at MAIAM through social media. Though I had financial difficulties, MAIAM helped me study by giving me a scholarship. MAIAM provided us with hostel facilities for free and also helped us work part-time to meet the food expenses. Today, after my one year of hard work, MAIAM allows me to be placed at PPS Mahindra. Rs 27,861.", Name = "Aaris Raja.A", Role = "Placed student" },
                    new() { Quote = "I choose MAIAM because it is the best institution for Auto Body Technician and the teachers are very qualified and I know they want us to succeed.", Name = "Ali Ashkar", Role = "Student" },
                    new() { Quote = "I had the opportunity to study at MAIAM and I'm really thankful to them because of the great teachers, staff always smiling and helping you with your stuff. Academically is great!", Name = "Muhammed Fasil", Role = "Student" },
                    new() { Quote = "My name is Arjun. I just want to say that this institution helps me learn a lot of things. I get a lot of help with the practical training.", Name = "Arjun", Role = "Student" },
                    new() { Quote = "Manappuram Institute of automotive skills(MAIAM) is a great institution. The teachers here are super about encouraging students to do their very best. Everyone here is destined for a great future.", Name = "Dhanush Babu", Role = "Student" },
                },
                GalleryPreviewImages = new List<string>
                {
                    "/images/gallery/gallery-1.jpg",
                    "/images/gallery/gallery-2.jpg",
                    "/images/gallery/gallery-3.jpg",
                    "/images/gallery/gallery-4.png",
                    "/images/gallery/gallery-5.jpg",
                    "/images/gallery/gallery-6.jpg",
                }
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
