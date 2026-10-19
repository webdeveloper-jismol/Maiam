namespace Maiamnew.Models
{
    public sealed record CourseSummary(
        string Name,
        string Path,
        string Summary,
        string Duration,
        string Certification,
        string Qualification,
        string ImageKey,
        bool InMainMenu);

    public sealed record Testimonial(string Name, string Role, string Quote, string ImageKey);

    public sealed record FacultyMember(
        string Name,
        string Designation,
        string Experience,
        IReadOnlyList<string> Achievements,
        string ImageKey);

    public sealed record GalleryAlbum(string Id, string Title, string? Note, IReadOnlyList<string> ImageKeys);

    public sealed record VideoItem(string YouTubeId, string Title, string Channel);

    public sealed record Facility(string Title, string? Description, string Icon);
}
