namespace Maiam.Models.ViewModels
{
    public class PlacementsViewModel
    {
        public List<StatItem> Stats { get; set; } = new();
        public List<PlacedStudent> PlacedStudents { get; set; } = new();
        public List<string> Recruiters { get; set; } = new();
    }

    public class PlacedStudent
    {
        public string Name { get; set; } = string.Empty;
        public string Batch { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Package { get; set; } = string.Empty;
    }
}
