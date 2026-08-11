namespace Maiam.Models.ViewModels
{
    public class FacultiesViewModel
    {
        public List<FacultyMember> Faculty { get; set; } = new();
    }

    public class FacultyMember
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
    }
}
