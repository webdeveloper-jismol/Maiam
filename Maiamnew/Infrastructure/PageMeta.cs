using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Maiamnew.Infrastructure
{
    public sealed record Breadcrumb(string Label, string? Path = null);

    /// <summary>
    /// Page-level SEO settings. Each view sets one with <c>ViewData.SetPageMeta(...)</c>;
    /// _SeoMeta renders the title, description, canonical, Open Graph and JSON-LD.
    /// </summary>
    public sealed class PageMeta
    {
        public required string Title { get; init; }

        public required string Description { get; init; }

        /// <summary>Canonical path with leading and trailing slash, e.g. "/about-maiam/".</summary>
        public required string Path { get; init; }

        public string OgImage { get; init; } = "/assets/images/og/default.jpg";

        public string OgType { get; init; } = "website";

        public IReadOnlyList<Breadcrumb> Breadcrumbs { get; init; } = [];

        public bool NoIndex { get; init; }

        /// <summary>Additional JSON-LD documents (already serialised) for this page.</summary>
        public IReadOnlyList<string> StructuredData { get; init; } = [];

        public string FullTitle => Path == "/" ? Title : $"{Title} | {SiteInfo.ShortName}";

        public string CanonicalUrl => SiteInfo.AbsoluteUrl(Path);
    }

    public static class PageMetaExtensions
    {
        private const string Key = "PageMeta";

        public static void SetPageMeta(this ViewDataDictionary viewData, PageMeta meta)
        {
            viewData[Key] = meta;
            viewData["Title"] = meta.FullTitle;
        }

        public static PageMeta? GetPageMeta(this ViewDataDictionary viewData) => viewData[Key] as PageMeta;
    }
}
