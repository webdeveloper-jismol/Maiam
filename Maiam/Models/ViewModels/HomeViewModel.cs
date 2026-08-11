namespace Maiam.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<HeroSlide> HeroSlides { get; set; } = new();
        public List<FeatureCard> FeatureCards { get; set; } = new();
        public List<CourseSummary> Courses { get; set; } = new();
        public List<StatItem> Stats { get; set; } = new();
        public List<TestimonialItem> Testimonials { get; set; } = new();
        public List<string> GalleryPreviewImages { get; set; } = new();
    }

    public class HeroSlide
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string Heading { get; set; } = string.Empty;
        public string Subheading { get; set; } = string.Empty;
    }

    public class FeatureCard
    {
        public string IconKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CourseSummary
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string CertificationTag { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Highlights { get; set; } = new();
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }

    public class StatItem
    {
        public int Value { get; set; }
        public string Suffix { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public class TestimonialItem
    {
        public string Quote { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
