using Maiam.Models;
using Maiam.Services;
using Maiam_DAL.DAL;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Register EF Core DbContext
builder.Services.AddDbContext<MaiamDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Maiam"));
});

// Register DAL service
builder.Services.AddScoped<Data_Layer>();

// Required for API calls (HttpClient)
builder.Services.AddHttpClient();

// Brevo transactional email (Contact form)
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, BrevoEmailService>();

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();   // REQUIRED
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();