namespace Maiamnew.Infrastructure
{
    /// <summary>
    /// Keeps the old WordPress URL structure working:
    ///  - 301-redirects retired WordPress pages and files to their new location;
    ///  - 301-redirects page URLs without a trailing slash to the canonical form
    ///    (WordPress always served pages as /slug/).
    /// </summary>
    public sealed class LegacyUrlMiddleware(RequestDelegate next)
    {
        private static readonly Dictionary<string, string> Redirects = new(StringComparer.OrdinalIgnoreCase)
        {
            // Pages that were merged into other pages
            ["/who-we-are/"] = "/about-maiam/#who-we-are",
            ["/video-gallery/"] = "/gallery/#videos",
            ["/certifications/"] = "/nsdc/",
            ["/accreditation-certificate/"] = "/certificate-of-affiliation/",

            // WordPress placeholders and the retired old home page
            ["/home-2/"] = "/",
            ["/sample-page/"] = "/",
            ["/sample-page-2/"] = "/",
            ["/hello-world/"] = "/",
            ["/hello-world-2/"] = "/",
            ["/category/uncategorized/"] = "/",
            ["/gallery1/"] = "/gallery/",
            ["/gallery2/"] = "/gallery/",
            ["/gallery3/"] = "/gallery/",
            ["/gallery4/"] = "/gallery/",

            // Original document location
            ["/wp-content/uploads/2022/02/QP-Auto-Body-Repair-Technician-level-4.pdf"] =
                "/assets/documents/qp-automotive-body-repair-technician-level-4.pdf",
        };

        public Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method))
            {
                return next(context);
            }

            var path = request.Path.Value ?? "/";
            var isFile = Path.HasExtension(path);
            var slashed = isFile || path.EndsWith('/') ? path : path + "/";

            if (Redirects.TryGetValue(slashed, out var target))
            {
                return Redirect(context, target);
            }

            if (!isFile && !path.EndsWith('/'))
            {
                return Redirect(context, slashed + request.QueryString);
            }

            return next(context);
        }

        private static Task Redirect(HttpContext context, string location)
        {
            context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
            context.Response.Headers.Location = location;
            return Task.CompletedTask;
        }
    }
}
