using System.Text.Json;

namespace Maiamnew.Infrastructure
{
    /// <summary>
    /// Builds schema.org JSON-LD documents. Only facts published on the original
    /// website are used: no ratings, reviews or accreditation claims are generated.
    /// </summary>
    public static class StructuredData
    {
        // The default encoder escapes <, > and &, so the output is safe inside a script block.
        private static readonly JsonSerializerOptions Options = new() { WriteIndented = false };

        private static string Serialize(object value) => JsonSerializer.Serialize(value, Options);

        public static Dictionary<string, object?> OrganizationNode() => new()
        {
            ["@type"] = "EducationalOrganization",
            ["@id"] = SiteInfo.BaseUrl + "/#organization",
            ["name"] = SiteInfo.Name,
            ["alternateName"] = SiteInfo.ShortName,
            ["url"] = SiteInfo.BaseUrl + "/",
            ["logo"] = SiteInfo.AbsoluteUrl("/assets/images/branding/maiam-icon-512.png"),
            ["image"] = SiteInfo.AbsoluteUrl("/assets/images/og/default.jpg"),
            ["foundingDate"] = SiteInfo.EstablishedYear.ToString(),
            ["email"] = SiteInfo.Email,
            ["telephone"] = SiteInfo.Phone1Tel,
            ["address"] = new Dictionary<string, object?>
            {
                ["@type"] = "PostalAddress",
                ["streetAddress"] = SiteInfo.AddressLine1,
                ["addressLocality"] = SiteInfo.Locality,
                ["addressRegion"] = SiteInfo.State,
                ["postalCode"] = SiteInfo.PostalCode,
                ["addressCountry"] = "IN",
            },
            ["geo"] = new Dictionary<string, object?>
            {
                ["@type"] = "GeoCoordinates",
                ["latitude"] = SiteInfo.Latitude,
                ["longitude"] = SiteInfo.Longitude,
            },
            ["parentOrganization"] = new Dictionary<string, object?>
            {
                ["@type"] = "Organization",
                ["name"] = SiteInfo.FoundationName,
                ["description"] = SiteInfo.FoundationDescriptor,
            },
            ["sameAs"] = new[] { SiteInfo.FacebookUrl, SiteInfo.InstagramUrl, SiteInfo.YouTubeUrl, SiteInfo.TwitterUrl },
        };

        public static string Organization()
        {
            var document = new Dictionary<string, object?> { ["@context"] = "https://schema.org" };
            foreach (var (key, value) in OrganizationNode())
            {
                document[key] = value;
            }
            return Serialize(document);
        }

        public static string WebSite() => Serialize(new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "WebSite",
            ["@id"] = SiteInfo.BaseUrl + "/#website",
            ["name"] = SiteInfo.Name,
            ["url"] = SiteInfo.BaseUrl + "/",
            ["publisher"] = new Dictionary<string, object?> { ["@id"] = SiteInfo.BaseUrl + "/#organization" },
        });

        public static string Breadcrumbs(IReadOnlyList<Breadcrumb> crumbs, string currentPath)
        {
            var items = new List<object>();
            var position = 1;
            items.Add(ListItem(position++, "Home", "/"));
            for (var i = 0; i < crumbs.Count; i++)
            {
                var isLast = i == crumbs.Count - 1;
                // Menu groupings without a page of their own (e.g. "Certifications") are left out.
                if (crumbs[i].Path is null && !isLast)
                {
                    continue;
                }
                items.Add(ListItem(position++, crumbs[i].Label, isLast ? currentPath : crumbs[i].Path!));
            }

            return Serialize(new Dictionary<string, object?>
            {
                ["@context"] = "https://schema.org",
                ["@type"] = "BreadcrumbList",
                ["itemListElement"] = items,
            });
        }

        private static Dictionary<string, object?> ListItem(int position, string name, string path) => new()
        {
            ["@type"] = "ListItem",
            ["position"] = position,
            ["name"] = name,
            ["item"] = SiteInfo.AbsoluteUrl(path),
        };

        /// <param name="duration">ISO 8601 duration of the programme, e.g. "P1Y" or "P3M".</param>
        /// <param name="credential">Credential named on the original course page.</param>
        public static string Course(string name, string description, string path, string duration, string credential) =>
            Serialize(new Dictionary<string, object?>
            {
                ["@context"] = "https://schema.org",
                ["@type"] = "Course",
                ["name"] = name,
                ["description"] = description,
                ["url"] = SiteInfo.AbsoluteUrl(path),
                ["inLanguage"] = "en",
                ["provider"] = new Dictionary<string, object?>
                {
                    ["@type"] = "EducationalOrganization",
                    ["@id"] = SiteInfo.BaseUrl + "/#organization",
                    ["name"] = SiteInfo.Name,
                    ["sameAs"] = SiteInfo.BaseUrl + "/",
                },
                ["educationalCredentialAwarded"] = credential,
                ["hasCourseInstance"] = new Dictionary<string, object?>
                {
                    ["@type"] = "CourseInstance",
                    ["courseMode"] = "Onsite",
                    ["courseWorkload"] = duration,
                    ["location"] = new Dictionary<string, object?>
                    {
                        ["@type"] = "Place",
                        ["name"] = SiteInfo.Name,
                        ["address"] = $"{SiteInfo.AddressLine1}, {SiteInfo.AddressLine2}, {SiteInfo.State} {SiteInfo.PostalCode}",
                    },
                },
            });
    }
}
