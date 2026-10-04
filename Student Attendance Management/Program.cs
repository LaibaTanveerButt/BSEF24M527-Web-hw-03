WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// Default page is Student/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.Run();
