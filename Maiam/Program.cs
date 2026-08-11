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

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // REQUIRED
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();