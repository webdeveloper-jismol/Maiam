using Maiamnew.Models;

namespace Maiamnew.Services
{
    /// <summary>
    /// Repeated content migrated from maiam.co.in (courses, testimonials, faculty,
    /// gallery albums and videos). Text is kept as published on the original site.
    /// </summary>
    public static class SiteContent
    {
        public static readonly IReadOnlyList<CourseSummary> Courses =
        [
            new("Automotive Body Repair Technician",
                "/courses/",
                "Train to repair the body and frame of vehicles damaged by major and minor dents, with internship training in dealership workshops.",
                "1 Year (8 months institutional + 4 months internship)",
                "ASDC, NSDC & NCVET",
                "SSLC or Plus Two",
                "automotive-body-repair/door-inspection",
                InMainMenu: true),
            new("Advanced Welding Technician",
                "/advanced-welding-technician/",
                "TIG, MIG/MAG and ARC welding including pipe welding. Regular and weekend batches are available.",
                "3 Months",
                "NSDC",
                "SSLC or Plus Two",
                "advanced-welding/instructor-guided-practice",
                InMainMenu: true),
            new("Two Wheeler Repair Technician",
                "/two-wheeler-repair-technician-course/",
                "Fundamental mechanical repairs essential for maintaining and fixing motorcycles and scooters.",
                "4 Months",
                "MAIAM & NSDC",
                "SSLC or Plus Two",
                "workshops/tool-board",
                InMainMenu: false),
            new("Welding Technician",
                "/welding-technician/",
                "Training in TIG, MIG, spot, electric and gas welding.",
                "1 Month",
                "MAIAM & NSDC",
                "SSLC or Plus Two",
                "advanced-welding/arc-closeup-wide",
                InMainMenu: false),
        ];

        public static IEnumerable<CourseSummary> MainCourses => Courses.Where(c => c.InMainMenu);

        public static IEnumerable<CourseSummary> RelatedTo(string path) => MainCourses.Where(c => c.Path != path);

        // Quotes are reproduced as published on the original home page.
        public static readonly IReadOnlyList<Testimonial> Testimonials =
        [
            new("Dhanush Babu", "Student",
                "Manappuram Institute of automotive skills (MAIAM) is a great institution. The teachers here are super about encouraging students to do their very best. Everyone here is destined for a great future.",
                "testimonials/dhanush-babu"),
            new("Aaris Raja.A", "Placed student",
                "Hi, I am Aris Raja from a hilly village in Idukki and I came to know about the automotive body repair technician course at MAIAM through social media. Though I had financial difficulties, MAIAM helped me study by giving me a scholarship. MAIAM provided us with hostel facilities for free and also helped us work part-time to meet the food expenses. Today, after my one year of hard work, MAIAM allows me to be a place at PPS Mahindra. Rs 27,861.",
                "testimonials/aaris-raja"),
            new("Ali Ashkar", "Student",
                "I choose MAIAM because it is the best institution for Auto Body Technician and the teachers are very qualified and I know they want us to succeed.",
                "testimonials/ali-ashkar"),
            new("Muhammed Fasil", "Student",
                "I had the opportunity to study at MAIAM and I’m really thankful to them because of the great teachers, staff always smiling and helping you with your stuff. Academically is grant!",
                "testimonials/muhammed-fasil"),
            new("Arjun", "Student",
                "My name is Arjun. I just want to say that this institution helps me learn a lot of things. I get a lot of help with the subjects and practical sections if I need help. Teachers are very patient with me, trying to take their time and teach me.",
                "testimonials/arjun"),
        ];

        public static readonly IReadOnlyList<FacultyMember> Faculty =
        [
            new("Mr. Girish Kumar", "Training Instructor",
                "21 years’ experience in Toyota, Lexus and GM (Abroad) in various companies and capacities such as Bodyshop Supervisor, Estimator, Service Advisor, Workshop Controller & Denter.",
                ["2011 Champion in the National Skill Contest (Japan)", "2009 Runner-up in the National Skill Contest"],
                "faculties/girish-kumar"),
            new("Mr. Ramesh Babu", "Training Instructor",
                "20 years’ experience as an Auto Body Repair and Body Training Instructor at Toyota and Lexus (Abroad).",
                ["National Skill Champion in 2003, Oman", "Visited Toyota Plant, Japan in 2004", "GCC Grand Prix finalist in 2003"],
                "faculties/ramesh-babu"),
            new("Mr. Kishore", "Training Instructor",
                "20 years’ experience as an Automotive Paint Technician (Abroad).",
                [],
                "faculties/kishore"),
        ];

        public static readonly IReadOnlyList<Facility> Facilities =
        [
            new("Computer Lab", "MAIAM Institution has well-furnished computer labs.", "monitor"),
            new("Hostel Facility", "Free accommodation for MAIAM students.", "bed"),
            new("Transportation", "Free transportation from the nearest bus stop to MAIAM.", "bus"),
            new("Soft Skills Empowerment", "MAIAM equips you with the skill of English communication that will launch you into a brilliant life of exciting opportunities.", "chat"),
            // The original page lists these four by title only.
            new("Smart Classroom", null, "screen"),
            new("Eco Friendly Campus", null, "leaf"),
            new("Disabled Friendly Campus and Restroom", null, "access"),
            new("Air Conditioned Classroom", null, "snow"),
        ];

        public static readonly IReadOnlyList<GalleryAlbum> GalleryAlbums =
        [
            new("appointment-letter-ceremony", "Appointment Letter Distribution Ceremony", null,
                ["gallery/appointment-letter-ceremony/appointment-letter-ceremony-01"]),
            new("toyota-plant-visit", "Toyota Plant Visit", null,
                ["gallery/toyota-plant-visit/toyota-plant-visit-01", "gallery/toyota-plant-visit/toyota-plant-visit-02"]),
            new("mahindra-factory-visit", "Mahindra Factory Visit – Coimbatore", null,
                ["placements/mahindra-factory-visit", "gallery/mahindra-factory-visit/mahindra-factory-visit-01"]),
            new("drug-awareness-session", "Drug Awareness Session", null,
                ["gallery/drug-awareness-session/drug-awareness-session-01"]),
            new("onam-2024", "Onam Celebrations 2024", null,
                Range("gallery/onam-2024/onam-2024", 10)),
            new("building-inauguration", "Renovated Office Building, Workshop & Classroom Inauguration", null,
                Range("gallery/building-inauguration/building-inauguration", 12)),
            new("new-batch-commencement", "Commencement of New Batch", null,
                Range("gallery/new-batch-commencement/new-batch-commencement", 8)),
            new("practical-training", "Practical Training & Campus", null,
            [
                "automotive-body-repair/students-door-panel",
                "workshops/students-repair-bay",
                "advanced-welding/instructor-guided-practice",
                "advanced-welding/student-welding",
                "automotive-body-repair/student-panel-work",
                "advanced-welding/floor-practice",
                "workshops/student-workbench",
                "workshops/students-hearing-protection",
                "workshops/tool-board",
                "workshops/paint-drying-equipment",
                "workshops/equipment-bay",
                "automotive-body-repair/quarter-panel-replacement",
                "automotive-body-repair/door-inspection",
                "automotive-body-repair/dent-repair-equipment",
                "workshops/students-tool-selection",
                "facilities/classroom-faculty",
                "facilities/classroom-address",
                "facilities/classroom-session",
                "facilities/classroom",
                "facilities/computer-lab",
                "facilities/laptop-classroom",
                "campus/campus-entrance-building",
                "campus/reception-lobby",
                "campus/seminar-room",
                "campus/campus-pathway",
            ]),
            new("festive-celebrations", "Festive Celebrations", null,
                Range("gallery/festive-celebrations/festive-celebrations", 8)),
            new("students-and-faculty", "Students & Faculty", null,
                Range("gallery/students-and-faculty/students-and-faculty", 9)),
        ];

        /// <summary>Videos embedded on the original Gallery and Video Gallery pages.</summary>
        public static readonly IReadOnlyList<VideoItem> GalleryVideos =
        [
            new("uQ79kUA4AKg", "Automotive Skill Course | Job Placement | MAIAM", "MAIAM"),
            new("LTe5-aD19K4", "Transforming Dreams into Careers | Manappuram Institute of Automotive Skills (MAIAM)", "MAIAM"),
            new("QgEMwiQQY9Y", "Manappuram Auto Body Technician Level 4 Course Brief", "Manappuram Foundation"),
            new("tSfozQnB3bw", "Manappuram Auto Body Technician Level 4 Course", "Manappuram Foundation"),
            new("jG3oUwu7Po8", "Manappuram Auto Body Technician Level 4 Course", "Manappuram Foundation"),
        ];

        /// <summary>Videos embedded on the original Testimonials page.</summary>
        public static readonly IReadOnlyList<VideoItem> TestimonialVideos =
        [
            new("twRkcq9ceXc", "A moment of pride for MAIAM!", "MAIAM"),
            new("mlEQrT9GTM0", "A moment of pride for MAIAM!", "MAIAM"),
            new("tY9r8cI_wA8", "A moment of pride for MAIAM!", "MAIAM"),
            new("Bjch6-yVamI", "A moment of pride for MAIAM!", "MAIAM"),
            new("cEp5ckbKek8", "A moment of pride for MAIAM!", "MAIAM"),
        ];

        private static string[] Range(string prefix, int count) =>
            Enumerable.Range(1, count).Select(i => $"{prefix}-{i:00}").ToArray();
    }
}
