namespace Maiam.Models.ViewModels
{
    public class GalleryImage
    {
        public string Url { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
    }

    public class GallerySection
    {
        public string Heading { get; set; } = string.Empty;
        public List<GalleryImage> Images { get; set; } = new();
    }

    public class GalleryViewModel
    {
        public List<GallerySection> Sections { get; set; } = new();
    }
}
