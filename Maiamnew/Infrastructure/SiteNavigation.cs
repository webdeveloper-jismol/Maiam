namespace Maiamnew.Infrastructure
{
    /// <summary>A menu entry. Entries without a path are dropdown parents only.</summary>
    public sealed record NavItem(string Label, string? Path, IReadOnlyList<NavItem>? Children = null)
    {
        public bool HasChildren => Children is { Count: > 0 };

        public bool IsActive(string currentPath) =>
            (Path is not null && string.Equals(Normalize(currentPath), Path, StringComparison.OrdinalIgnoreCase))
            || (Children?.Any(c => c.IsActive(currentPath)) ?? false);

        private static string Normalize(string path) => path.EndsWith('/') ? path : path + "/";
    }

    /// <summary>
    /// Navigation migrated from maiam.co.in in its original order and labels.
    /// Original URLs are preserved so existing search results and links keep working.
    /// </summary>
    public static class SiteNavigation
    {
        public static readonly IReadOnlyList<NavItem> Main =
        [
            new("Home", "/"),
            new("About Us", null,
            [
                new("Objectives", "/objectives/"),
                new("About MAIAM", "/about-maiam/"),
                new("Vision and Mission", "/vision-and-mission/"),
                new("Testimonials", "/testimonials/"),
                new("Certifications", null,
                [
                    new("NSDC", "/nsdc/"),
                    new("ASDC, NCVET Affiliation Certificate", "/certificate-of-affiliation/"),
                ]),
            ]),
            new("Courses", null,
            [
                new("Automotive Body Repair Technician", "/courses/"),
                new("Advanced Welding Technician", "/advanced-welding-technician/"),
            ]),
            new("Placements", "/our-placements/"),
            new("Faculties", "/faculties/"),
            new("Our Facilities", "/our-facilities/"),
            new("Gallery", "/gallery/"),
            new("Contact Us", "/contact-us/"),
        ];

        /// <summary>Footer quick links, as listed in the original footer.</summary>
        public static readonly IReadOnlyList<NavItem> QuickLinks =
        [
            new("About Us", "/about-maiam/"),
            new("Courses", "/courses/"),
            new("Gallery", "/gallery/"),
            new("Contact Us", "/contact-us/"),
            new("Faculties", "/faculties/"),
            new("Our Vision", "/vision-and-mission/"),
        ];

        public static readonly IReadOnlyList<NavItem> Courses =
        [
            new("Automotive Body Repair Technician", "/courses/"),
            new("Advanced Welding Technician", "/advanced-welding-technician/"),
        ];

        /// <summary>Every indexable page, used for sitemap.xml.</summary>
        public static readonly IReadOnlyList<string> IndexablePaths =
        [
            "/",
            "/objectives/",
            "/about-maiam/",
            "/vision-and-mission/",
            "/testimonials/",
            "/directors-message/",
            "/nsdc/",
            "/certificate-of-affiliation/",
            "/courses/",
            "/advanced-welding-technician/",
            "/two-wheeler-repair-technician-course/",
            "/welding-technician/",
            "/our-placements/",
            "/faculties/",
            "/our-facilities/",
            "/gallery/",
            "/contact-us/",
            "/register-now/",
        ];
    }
}
