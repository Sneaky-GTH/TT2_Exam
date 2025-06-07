using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TT2_Exam.Models;

namespace TT2_Exam.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(
        AppDbContext context,
        UserManager<UserModel> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        
        context.Database.Migrate();
        
        // === 1. Seed Roles ===
        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // === 2. Seed Default Admin User ===
        string adminUsername = "admin";
        string adminEmail = "admin@admin.admin";
        string adminPassword = "Admin$1";
        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var adminUser = new UserModel
            {
                UserName = adminUsername,
                Email = adminEmail,
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // === 3. Seed Default Normal User ===
        string userUsername = "user";
        string userEmail = "user@example.com";
        string userPassword = "User$1";
        if (await userManager.FindByEmailAsync(userEmail) is null)
        {
            var normalUser = new UserModel
            {
                UserName = userUsername,
                Email = userEmail,
            };

            var result = await userManager.CreateAsync(normalUser, userPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, "User");
            }
        }

        // Return if already seeded
        if (context.VideoGames.Any()) return;

        var categories = new List<CategoryModel>
        {
            new CategoryModel { Name = "RPG" },
            new CategoryModel { Name = "Simulation" },
            new CategoryModel { Name = "Indie" },
            new CategoryModel { Name = "Action" },
            new CategoryModel { Name = "Farming" },
            new CategoryModel { Name = "Factory" },
        };

        context.Categories.AddRange(categories);
        context.SaveChanges(); // Save so that categories get IDs

        var games = new List<VideoGameModel>
        {
            new VideoGameModel { Title = "Elden Ring", Price = 59.99M },
            new VideoGameModel { Title = "Stardew Valley", Price = 14.99M },
            new VideoGameModel { Title = "Factorio", Price = 29.99M },
            new VideoGameModel { Title = "The Witcher 3: Wild Hunt", Price = 39.99M },
            new VideoGameModel { Title = "Cyberpunk 2077", Price = 49.99M },
            new VideoGameModel { Title = "Hades", Price = 24.99M },
            new VideoGameModel { Title = "Celeste", Price = 19.99M },
            new VideoGameModel { Title = "Dark Souls III", Price = 39.99M },
            new VideoGameModel { Title = "Among Us", Price = 4.99M },
            new VideoGameModel { Title = "Minecraft", Price = 26.95M },
            new VideoGameModel { Title = "Terraria", Price = 9.99M },
            new VideoGameModel { Title = "Slay the Spire", Price = 24.99M },
            new VideoGameModel { Title = "Portal 2", Price = 9.99M },
            new VideoGameModel { Title = "Cuphead", Price = 19.99M },
            new VideoGameModel { Title = "Hollow Knight", Price = 14.99M },
            new VideoGameModel { Title = "Overwatch", Price = 39.99M },
            new VideoGameModel { Title = "DOOM Eternal", Price = 59.99M },
            new VideoGameModel { Title = "Red Dead Redemption 2", Price = 59.99M },
            new VideoGameModel { Title = "The Legend of Zelda: Breath of the Wild", Price = 59.99M },
            new VideoGameModel { Title = "Animal Crossing: New Horizons", Price = 59.99M },
            new VideoGameModel { Title = "Final Fantasy VII Remake", Price = 59.99M },
            new VideoGameModel { Title = "Ghost of Tsushima", Price = 59.99M },
            new VideoGameModel { Title = "Valorant", Price = 0.00M },
            new VideoGameModel { Title = "League of Legends", Price = 0.00M },
            new VideoGameModel { Title = "Fortnite", Price = 0.00M },
            new VideoGameModel { Title = "Apex Legends", Price = 0.00M },
            new VideoGameModel { Title = "Metro Exodus", Price = 39.99M },
            new VideoGameModel { Title = "Resident Evil Village", Price = 59.99M },
            new VideoGameModel { Title = "Assassin's Creed Valhalla", Price = 59.99M },
            new VideoGameModel { Title = "Fall Guys", Price = 19.99M },
        };

        context.VideoGames.AddRange(games);
        context.SaveChanges(); // Save so that games get IDs
        
        context.GameSpecificCategories.AddRange(
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Elden Ring").Id,
                CategoryId = categories.First(c => c.Name == "RPG").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Factorio").Id,
                CategoryId = categories.First(c => c.Name == "Factory").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Elden Ring").Id,
                CategoryId = categories.First(c => c.Name == "Action").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Factorio").Id,
                CategoryId = categories.First(c => c.Name == "Action").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Stardew Valley").Id,
                CategoryId = categories.First(c => c.Name == "Farming").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Stardew Valley").Id,
                CategoryId = categories.First(c => c.Name == "Indie").Id
            }
        );

        context.SaveChanges();
    }
}
