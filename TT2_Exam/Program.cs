using TT2_Exam.Data;
using Microsoft.EntityFrameworkCore;


using TT2_Exam.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------- Configure Services ----------
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// ---------- Build App ----------
var app = builder.Build();

// ---------- Seed Database ----------
using (var scope = app.Services.CreateScope())
{   
    
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    
    // Rebuilding the database every launch for testing
    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
        DataSeeder.Seed(dbContext); 
    }

}

// ---------- Configure Middleware ----------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS = HTTP Strict Transport Security
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// ---------- Configure Routing ----------
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ---------- Run the App ----------
app.Run();
