using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maiamnew.Infrastructure
{
    /// <summary>
    /// Renders an optimised catalog image with srcset, intrinsic size and lazy loading.
    /// Usage: &lt;maiam-img key="campus/reception-lobby" sizes="(min-width: 960px) 50vw, 100vw" class="..." /&gt;
    /// Omit alt to use the catalog description; pass alt="" for decorative images.
    /// </summary>
    [HtmlTargetElement("maiam-img", TagStructure = TagStructure.WithoutEndTag)]
    public class ResponsiveImageTagHelper : TagHelper
    {
        public string Key { get; set; } = string.Empty;

        public string? Alt { get; set; }

        public string Sizes { get; set; } = "100vw";

        /// <summary>Above-the-fold images load eagerly with high fetch priority.</summary>
        public bool Eager { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var image = ImageCatalog.Get(Key);

            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.SetAttribute("src", image.DefaultUrl);
            if (image.Widths.Count > 1)
            {
                output.Attributes.SetAttribute("srcset", image.SrcSet);
                output.Attributes.SetAttribute("sizes", Sizes);
            }
            output.Attributes.SetAttribute("width", image.Width);
            output.Attributes.SetAttribute("height", image.Height);
            output.Attributes.SetAttribute("alt", Alt ?? image.Alt);
            output.Attributes.SetAttribute("decoding", "async");
            output.Attributes.SetAttribute("loading", Eager ? "eager" : "lazy");
            if (Eager)
            {
                output.Attributes.SetAttribute("fetchpriority", "high");
            }
        }
    }
}
