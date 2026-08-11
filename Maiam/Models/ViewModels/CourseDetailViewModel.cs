namespace Maiam.Models.ViewModels
{
    public class CourseDetailViewModel
    {
        public string CertificationTag { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Certification { get; set; } = string.Empty;
        public List<string> KeyHighlights { get; set; } = new();
        public List<string> CurriculumModules { get; set; } = new();

        // Auto Body Repair uses CareerPaths; Advanced Welding uses Batches
        public List<CareerPathItem> CareerPaths { get; set; } = new();
        public List<CourseBatch> Batches { get; set; } = new();
        public string ThirdTabLabel { get; set; } = string.Empty;
    }

    public class CareerPathItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CourseBatch
    {
        public string Name { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
    }
}
