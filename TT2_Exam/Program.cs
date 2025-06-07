using Microsoft.AspNetCore.Identity;
using TT2_Exam.Data;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------- Configure Services ----------
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddDefaultIdentity<UserModel>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// ---------- Build App ----------
var app = builder.Build();

// ---------- Seed Database ----------
using (var scope = app.Services.CreateScope())
{   
    
    var services = scope.ServiceProvider;
    
    var dbContext = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<UserModel>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
        
        await DataSeeder.SeedAsync(dbContext, userManager, roleManager);
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
app.UseAuthentication();
app.UseAuthorization();

// ---------- Configure Routing ----------
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

app.MapRazorPages();

// ---------- Run the App ----------
app.Run();
