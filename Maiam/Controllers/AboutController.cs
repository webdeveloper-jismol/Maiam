using Maiam.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maiam.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult AboutMaiam()
        {
            ViewData["Title"] = "About MAIAM";
            ViewData["Description"] = "MAIAM is an initiative of Manappuram Foundation, Kerala's 1st Central Government-approved, ASDC-accredited automotive skills institution.";

            var model = new AboutMaiamViewModel
            {
                Strengths = new List<IconTextCard>
                {
                    new() { IconKey = "shield", Title = "Kerala's 1st Government Approved", Description = "Central Government-approved, ASDC-accredited institution in Automotive Body Repair Course" },
                    new() { IconKey = "trainer", Title = "25 Years Experienced Trainers", Description = "Learn from industry experts with decades of hands-on experience" },
                    new() { IconKey = "placement", Title = "100% Placement Support", Description = "All India based dealership placements with excellent starting packages" },
                    new() { IconKey = "workshop", Title = "Fully Mechanized Practical Lab", Description = "State-of-the-art workshop with modern equipment and tools" },
                    new() { IconKey = "scholarship", Title = "BPL/APL Scholarship", Description = "10% scholarship for BPL & 5% for APL backward students" },
                    new() { IconKey = "location", Title = "Prime Location", Description = "Located at Valapad, Thrissur with easy accessibility for students across Kerala." },
                    new() { IconKey = "scholarship", Title = "Scholarship Support", Description = "Financial assistance available for eligible BPL and APL backward students." },
                    new() { IconKey = "certificate", Title = "Industry Recognition", Description = "Certifications recognized by NCVET, ASDC, and NSDC across India." },
                }
            };

            return View(model);
        }

        public IActionResult VisionMission()
        {
            ViewData["Title"] = "Vision & Mission";
            ViewData["Description"] = "MAIAM's vision and mission to become India's leading automotive skills training institute, and the core values that guide our training.";

            var model = new VisionMissionViewModel
            {
                CoreValues = new List<IconTextCard>
                {
                    new() { IconKey = "excellence", Title = "Excellence", Description = "We strive for excellence in every aspect of training and student development." },
                    new() { IconKey = "integrity", Title = "Integrity", Description = "Honesty, transparency, and ethical conduct guide all our operations." },
                    new() { IconKey = "student", Title = "Student First", Description = "Every decision is made with the success and well-being of our students in mind." },
                    new() { IconKey = "innovation", Title = "Innovation", Description = "We continuously update our curriculum to reflect the latest industry trends." },
                    new() { IconKey = "collaboration", Title = "Collaboration", Description = "Strong partnerships with industry leaders ensure relevant, job-ready training." },
                    new() { IconKey = "inclusivity", Title = "Inclusivity", Description = "We welcome students from all backgrounds with scholarships and support." },
                }
            };

            return View(model);
        }

        public IActionResult Objectives()
        {
            ViewData["Title"] = "Objectives";
            ViewData["Description"] = "The objectives that drive MAIAM's training programs, certification, placement support and student welfare initiatives.";

            var model = new ObjectivesViewModel
            {
                Goals = new List<IconTextCard>
                {
                    new() { IconKey = "certificate", Title = "Government-Approved Certification", Description = "To provide Central Government-approved, ASDC-accredited training that is recognized across India and internationally." },
                    new() { IconKey = "workshop", Title = "Hands-On Practical Training", Description = "To ensure every student receives extensive practical workshop experience with modern tools and industry-standard equipment." },
                    new() { IconKey = "placement", Title = "100% Placement Support", Description = "To guarantee placement assistance with leading automotive dealerships and companies across India." },
                    new() { IconKey = "scholarship", Title = "Scholarship Accessibility", Description = "To make quality education accessible by offering scholarships for BPL (10%) and APL backward students (5%)." },
                    new() { IconKey = "trainer", Title = "Expert Faculty Development", Description = "To continuously upgrade faculty skills and knowledge to maintain the highest standards of teaching." },
                    new() { IconKey = "collaboration", Title = "Industry Partnerships", Description = "To build and maintain strong partnerships with automotive industry leaders for internships and placements." },
                    new() { IconKey = "student", Title = "Student Welfare", Description = "To provide hostel facilities, food support, and part-time work opportunities for students in need." },
                    new() { IconKey = "innovation", Title = "Continuous Improvement", Description = "To regularly update curriculum and training methodologies based on evolving industry requirements." },
                }
            };

            return View(model);
        }

        public IActionResult Testimonials()
        {
            ViewData["Title"] = "Testimonials";
            ViewData["Description"] = "Read what MAIAM students and placed graduates say about their training and career journey.";

            var model = new TestimonialsPageViewModel
            {
                Featured = new TestimonialItem
                {
                    Quote = "Hi, I am Aris Raja from a hilly village in Idukki and I came to know about the automotive body repair technician course at MAIAM through social media. Though I had financial difficulties, MAIAM helped me study by giving me a scholarship. MAIAM provided us with hostel facilities for free and also helped us work part-time to meet the food expenses. Today, after my one year of hard work, MAIAM allows me to be placed at PPS Mahindra. Rs 27,861.",
                    Name = "Aaris Raja.A",
                    Role = "Placed student"
                },
                Stories = new List<TestimonialItem>
                {
                    new() { Quote = "Hi, I am Aris Raja from a hilly village in Idukki and I came to know about the automotive body repair technician course at MAIAM through social media. Though I had financial difficulties, MAIAM helped me study by giving me a scholarship. MAIAM provided us with hostel facilities for free and also helped us work part-time to meet the food expenses. Today, after my one year of hard work, MAIAM allows me to be placed at PPS Mahindra. Rs 27,861.", Name = "Aaris Raja.A", Role = "Placed student" },
                    new() { Quote = "I choose MAIAM because it is the best institution for Auto Body Technician and the teachers are very qualified and I know they want us to succeed.", Name = "Ali Ashkar", Role = "Student" },
                    new() { Quote = "I had the opportunity to study at MAIAM and I'm really thankful to them because of the great teachers, staff always smiling and helping you with your stuff. Academically is great!", Name = "Muhammed Fasil", Role = "Student" },
                    new() { Quote = "My name is Arjun. I just want to say that this institution helps me learn a lot of things. I get a lot of help with the practical training.", Name = "Arjun", Role = "Student" },
                    new() { Quote = "Manappuram Institute of automotive skills(MAIAM) is a great institution. The teachers here are super about encouraging students to do their very best. Everyone here is destined for a great future.", Name = "Dhanush Babu", Role = "Student" },
                }
            };

            return View(model);
        }

        public IActionResult Certifications()
        {
            ViewData["Title"] = "Certifications";
            ViewData["Description"] = "MAIAM's accreditations with NSDC, ASDC and NCVET — nationally recognized certifications for automotive and welding skills training.";

            var model = new CertificationsViewModel
            {
                Certifications = new List<CertificationBlock>
                {
                    new()
                    {
                        Code = "NSDC",
                        Name = "National Skill Development Corporation",
                        Description = "The National Skill Development Corporation (NSDC) is a public-private partnership that aims to promote skill development in India. MAIAM is a registered training partner of NSDC, ensuring that our courses meet national occupational standards.",
                        Highlights = new List<string> { "Government-recognized certification", "Standardized curriculum aligned with NSQF", "Industry-relevant skill benchmarks" }
                    },
                    new()
                    {
                        Code = "ASDC",
                        Name = "Automotive Skill Development Council",
                        Description = "ASDC is the apex body for the automotive industry in India, established under the aegis of NSDC. MAIAM is an ASDC-accredited training center, making us Kerala's first institution to achieve this recognition for Automotive Body Repair Technician courses.",
                        Highlights = new List<string> { "1st ASDC-accredited institute in Kerala", "Automotive industry-approved curriculum", "Direct industry relevance and recognition" }
                    },
                    new()
                    {
                        Code = "NCVET",
                        Name = "National Council for Vocational Education & Training",
                        Description = "NCVET is the regulatory body for vocational education and training in India. Our affiliation with NCVET ensures that all our certifications are nationally recognized and hold value across all states and industries.",
                        Highlights = new List<string> { "National-level certification validity", "Regulated quality standards", "Accepted by employers nationwide" }
                    },
                }
            };

            return View(model);
        }
    }
}
