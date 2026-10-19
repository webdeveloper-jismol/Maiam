using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.RateLimiting;
using Maiamnew.Infrastructure;
using Maiamnew.Services;
using Maiamnew_DAL.DAL;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.WebEncoders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Register EF Core DbContext
builder.Services.AddDbContext<MaiamnewDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Maiamnew"));
});

// Register DAL service
builder.Services.AddScoped<Data_Layer>();

// Required for API calls (HttpClient)
builder.Services.AddHttpClient();

// Generate lowercase URLs with a trailing slash, matching the original WordPress URLs
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

// Compress HTML, CSS, JS, JSON and SVG responses (Brotli, then Gzip)
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["image/svg+xml"]);
});

// Throttle form submissions per client IP
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("forms", context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(10) }));
});

// Register Now enquiries are emailed. SMTP settings come from the "Email" configuration
// section (user secrets / environment variables) – see docs/EMAIL_SETUP.md.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEnquiryEmailSender, SmtpEnquiryEmailSender>();

// Output non-ASCII text (–, ’, ₹, Malayalam) as UTF-8 instead of numeric entities
builder.Services.Configure<WebEncoderOptions>(options =>
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseMiddleware<SecurityHeadersMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/");
    app.UseHsts();
}

app.UseMiddleware<LegacyUrlMiddleware>();
app.UseStatusCodePagesWithReExecute("/error/{0}/");

app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
    // Not in Development: Visual Studio's Browser Link / Hot Reload append script tags to
    // HTML after it is compressed, which corrupts the page (blank page in the browser).
    app.UseResponseCompression();
}
app.UseStaticFiles(new StaticFileOptions   // REQUIRED
{
    OnPrepareResponse = context =>
    {
        // CSS/JS referenced with asp-append-version (?v=hash) never change at that URL;
        // images, fonts and videos can be cached for 30 days, anything else for 7 days.
        var request = context.Context.Request;
        context.Context.Response.Headers[HeaderNames.CacheControl] =
            request.Query.ContainsKey("v") ? "public, max-age=31536000, immutable"
            : request.Path.StartsWithSegments("/assets") ? "public, max-age=2592000"
            : "public, max-age=604800";
    },
});
app.UseRouting();

app.UseRateLimiter();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
