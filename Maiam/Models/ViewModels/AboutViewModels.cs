namespace Maiam.Models.ViewModels
{
    public class IconTextCard
    {
        public string IconKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class AboutMaiamViewModel
    {
        public List<IconTextCard> Strengths { get; set; } = new();
    }

    public class VisionMissionViewModel
    {
        public List<IconTextCard> CoreValues { get; set; } = new();
    }

    public class ObjectivesViewModel
    {
        public List<IconTextCard> Goals { get; set; } = new();
    }

    public class TestimonialsPageViewModel
    {
        public TestimonialItem Featured { get; set; } = new();
        public List<TestimonialItem> Stories { get; set; } = new();
    }

    public class CertificationBlock
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Highlights { get; set; } = new();
    }

    public class CertificationsViewModel
    {
        public List<CertificationBlock> Certifications { get; set; } = new();
    }
}
