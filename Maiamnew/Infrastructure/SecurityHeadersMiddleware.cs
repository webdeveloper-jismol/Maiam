namespace Maiamnew.Infrastructure
{
    /// <summary>
    /// Adds baseline security headers. The Content Security Policy only allows the
    /// external services the site embeds: YouTube (privacy-enhanced) and Google Maps.
    /// In Development it also allows Visual Studio's Browser Link / Hot Reload, which
    /// load scripts and open connections on localhost ports.
    /// </summary>
    public sealed class SecurityHeadersMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        private readonly string _contentSecurityPolicy = string.Join("; ",
            "default-src 'self'",
            environment.IsDevelopment() ? "script-src 'self' http://localhost:*" : "script-src 'self'",
            "style-src 'self'",
            "img-src 'self' data: https://i.ytimg.com",
            "font-src 'self'",
            "media-src 'self'",
            "frame-src https://www.youtube-nocookie.com https://www.google.com",
            environment.IsDevelopment()
                ? "connect-src 'self' http://localhost:* ws://localhost:* wss://localhost:*"
                : "connect-src 'self'",
            "object-src 'none'",
            "base-uri 'self'",
            "form-action 'self'",
            "frame-ancestors 'self'");

        public Task InvokeAsync(HttpContext context)
        {
            var headers = context.Response.Headers;
            headers.XContentTypeOptions = "nosniff";
            headers.XFrameOptions = "SAMEORIGIN";
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=()";
            headers.ContentSecurityPolicy = _contentSecurityPolicy;
            return next(context);
        }
    }
}
