using Maiamnew.Infrastructure;

namespace Maiamnew.ViewModels
{
    /// <summary>Inner-page hero: breadcrumb, eyebrow, H1, lead and an optional photograph.</summary>
    public sealed record PageHeroModel(
        string Title,
        string? Lead,
        string? ImageKey,
        IReadOnlyList<Breadcrumb> Breadcrumbs,
        string? Eyebrow = null,
        string? ImageAlt = null);

    public sealed record VideoEmbedModel(string YouTubeId, string Title, bool Compact = false);

    /// <summary>A lightbox image tile. Alt/Caption default to the catalog description.</summary>
    public sealed record ImageTileModel(
        string Key,
        string Group,
        string Sizes = "(min-width: 960px) 30vw, 46vw",
        string? Alt = null,
        string? Caption = null,
        string? CssClass = null);
}
