using Microsoft.EntityFrameworkCore;
using TT2_Exam.Models;

namespace TT2_Exam.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Ensure database exists
        context.Database.Migrate();

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
