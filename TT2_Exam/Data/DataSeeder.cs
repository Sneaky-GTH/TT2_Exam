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
        
        await context.Database.MigrateAsync();
        
        // seed roles
        string[] roles = { "Admin", "User", "Publisher" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // seed admin
        const string adminUsername = "admin";
        const string adminEmail = "admin@admin.admin";
        const string adminPassword = "Admin$1";
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
        
        // seed a default user
        const string userUsername = "user";
        const string userEmail = "user@example.com";
        const string userPassword = "User$1";
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
        
        // seed another default user
        const string user2Username = "user2";
        const string user2Email = "user2@example.com";
        const string user2Password = "User$1";
        if (await userManager.FindByEmailAsync(user2Email) is null)
        {
            var normalUser = new UserModel
            {
                UserName = user2Username,
                Email = user2Email,
            };

            var result = await userManager.CreateAsync(normalUser, user2Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, "User");
            }
        }
        
        // seed a publisher user
        const string pubUsername = "BigPublishing";
        const string pubEmail = "contact@publishing.big";
        const string pubPassword = "User$1";
        if (await userManager.FindByEmailAsync(pubEmail) is null)
        {
            var normalUser = new UserModel
            {
                UserName = pubUsername,
                Email = pubEmail,
            };

            var result = await userManager.CreateAsync(normalUser, pubPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, "Publisher");
            }
        }
        
        const string pubUsername2 = "SmallPublishing";
        const string pubEmail2 = "contact@publishing.small";
        const string pubPassword2 = "User$1";
        if (await userManager.FindByEmailAsync(pubEmail2) is null)
        {
            var normalUser = new UserModel
            {
                UserName = pubUsername2,
                Email = pubEmail2,
            };

            var result = await userManager.CreateAsync(normalUser, pubPassword2);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, "Publisher");
            }
        }
        
        var userOne = await userManager.FindByEmailAsync(userEmail);
        var userOneId = userOne!.Id;
        
        var userTwo = await userManager.FindByEmailAsync(user2Email);
        var userTwoId = userTwo!.Id;

        var publisherOne = await userManager.FindByEmailAsync(pubEmail);
        var publisherOneId = publisherOne!.Id;
        
        var publisherTwo = await userManager.FindByEmailAsync(pubEmail2);
        var publisherTwoId = publisherTwo!.Id;

        var categories = new List<CategoryModel>
        {
            new CategoryModel { Name = "RPG" },
            new CategoryModel { Name = "Indie" },
            new CategoryModel { Name = "Action" },
            new CategoryModel { Name = "Factory" },
            new CategoryModel { Name = "Story" },
            new CategoryModel { Name = "Fighting Game" },
            new CategoryModel { Name = "Roguelike" },
            new CategoryModel { Name = "Apocalypse" },
            new CategoryModel { Name = "Shooter" },
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync(); // Save so that categories get IDs

        var games = new List<VideoGameModel>
        {
            new VideoGameModel
            {
                Title = "Factorio", PublisherId = publisherOneId, Price = 35.00M, ShortDescription = "A factory building game.", ThumbnailPath = "~/img/factorio_thumbnail.jpg",
                Description = "**Factorio** is a game in which you build and maintain factories. You will be mining resources, researching technologies, building infrastructure, automating production and fighting enemies. In the beginning you will find yourself chopping trees, mining ores and crafting mechanical arms and transport belts by hand, but in short time you can become an industrial powerhouse, with huge solar fields, oil refining and cracking, manufacture and deployment of construction and logistic robots, all for your resource needs. However this heavy exploitation of the planet's resources does not sit nicely with the locals, so you will have to be prepared to defend yourself and your machine empire." +
                              "\n\n" +
                              "Join forces with other players in cooperative Multiplayer, create huge factories, collaborate and delegate tasks between you and your friends. Add mods to increase your enjoyment, from small tweak and helper mods to complete game overhauls, Factorio's ground-up Modding support has allowed content creators from around the world to design interesting and innovative features. While the core gameplay is in the form of the freeplay scenario, there are a range of interesting challenges in the form of Scenarios. If you don't find any maps or scenarios you enjoy, you can create your own with the in-game **Map Editor**, place down entities, enemies, and terrain in any way you like, and even add your own custom script to make for interesting gameplay."
            },
            new VideoGameModel { Title = "Hi-Fi Rush", PublisherId = publisherOneId, Price = 39.99M, ShortDescription = "Beat up robots to the rhythm!", ThumbnailPath = "~/img/hifirush_thumbnail.jpg"},
            new VideoGameModel { Title = "Cyberpunk 2077", PublisherId = publisherOneId, Price = 49.99M, ShortDescription = "Explore Night City - become a legend.", ThumbnailPath = "~/img/cyberpunk_thumbnail.jpg"},
            new VideoGameModel { Title = "Yakuza 0", PublisherId = publisherOneId, Price = 19.99M, ShortDescription = "The glitz, glamour, and unbridled decadence of the 80s are back in Yakuza 0.", ThumbnailPath = "~/img/yakuza0_thumbnail.png"},
            new VideoGameModel { Title = "Fallout: New Vegas", PublisherId = publisherOneId, Price = 9.99M, ShortDescription = "Explore post-apocalyptic America and choose the fate of New Vegas.", ThumbnailPath = "~/img/fnv_thumbnail.jpg"},
            new VideoGameModel { Title = "Hades", Price = 24.99M, PublisherId = publisherOneId, ShortDescription = "Fight. Die. Repeat. Reach the Surface.", ThumbnailPath = "~/img/hades_thumbnail.jpeg"},
            new VideoGameModel { Title = "Balatro", Price = 9.99M, PublisherId = publisherTwoId, ShortDescription = "A poker roguelike.", ThumbnailPath = "~/img/balatro_thumbnail.png"},
            new VideoGameModel { Title = "HELLDIVERS™ 2", PublisherId = publisherTwoId, Price = 39.99M, ShortDescription = "For Super-Earth!", ThumbnailPath = "~/img/hd2_thumbnail.jpg"},
            new VideoGameModel { Title = "Hades II", PublisherId = publisherTwoId, Price = 24.99M, ShortDescription = "The sequel to 'Hades' is finally here!", ThumbnailPath = "~/img/hades2_thumbnail.jpg"},
            new VideoGameModel { Title = "Disco Elysium", PublisherId = publisherTwoId, Price = 26.95M, ShortDescription = "Be a detective and solve the case, all while trying not to die from alcoholism.", ThumbnailPath = "~/img/discoelysium_thumbnail.jpg"},
            new VideoGameModel { Title = "Nine Sols", PublisherId = publisherTwoId, Price = 19.99M, ThumbnailPath = "~/img/ninesols_thumbnail.jpg"},
            new VideoGameModel { Title = "GUILTY GEAR -STRIVE-", PublisherId = publisherTwoId, Price = 29.99M, ThumbnailPath = "~/img/ggst_thumbnail.jpg"},
        };

        context.VideoGames.AddRange(games);
        await context.SaveChangesAsync(); // Save so that games get IDs
        
        context.GameSpecificCategories.AddRange(
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Cyberpunk 2077").Id,
                CategoryId = categories.First(c => c.Name == "RPG").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Cyberpunk 2077").Id,
                CategoryId = categories.First(c => c.Name == "Shooter").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Fallout: New Vegas").Id,
                CategoryId = categories.First(c => c.Name == "RPG").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Fallout: New Vegas").Id,
                CategoryId = categories.First(c => c.Name == "Apocalypse").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Factorio").Id,
                CategoryId = categories.First(c => c.Name == "Factory").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Factorio").Id,
                CategoryId = categories.First(c => c.Name == "Indie").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Hi-Fi Rush").Id,
                CategoryId = categories.First(c => c.Name == "Action").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Hi-Fi Rush").Id,
                CategoryId = categories.First(c => c.Name == "Indie").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Yakuza 0").Id,
                CategoryId = categories.First(c => c.Name == "RPG").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Yakuza 0").Id,
                CategoryId = categories.First(c => c.Name == "Story").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Hades").Id,
                CategoryId = categories.First(c => c.Name == "Roguelike").Id
            },
            new GameSpecificCategoryModel        
            {
                VideoGameId = games.First(g => g.Title == "Hades").Id,
                CategoryId = categories.First(c => c.Name == "Action").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Hades").Id,
                CategoryId = categories.First(c => c.Name == "Indie").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Hades II").Id,
                CategoryId = categories.First(c => c.Name == "Roguelike").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Hades II").Id,
                CategoryId = categories.First(c => c.Name == "Action").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Balatro").Id,
                CategoryId = categories.First(c => c.Name == "Indie").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Balatro").Id,
                CategoryId = categories.First(c => c.Name == "Roguelike").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Disco Elysium").Id,
                CategoryId = categories.First(c => c.Name == "RPG").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Disco Elysium").Id,
                CategoryId = categories.First(c => c.Name == "Story").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Nine Sols").Id,
                CategoryId = categories.First(c => c.Name == "Indie").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "Nine Sols").Id,
                CategoryId = categories.First(c => c.Name == "Action").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "HELLDIVERS™ 2").Id,
                CategoryId = categories.First(c => c.Name == "Shooter").Id
            },
            new GameSpecificCategoryModel
            {
                VideoGameId = games.First(g => g.Title == "GUILTY GEAR -STRIVE-").Id,
                CategoryId = categories.First(c => c.Name == "Fighting Game").Id
            }
        );

        await context.SaveChangesAsync();

        context.UserLibrary.AddRange(
            new UserLibraryItemModel
            {
                UserId = userOneId,
                VideoGameId = games.First(g => g.Title == "Factorio").Id,
            },
            new UserLibraryItemModel
            {
                UserId = userTwoId,
                VideoGameId = games.First(g => g.Title == "Factorio").Id,
            }
        );

        context.Reviews.AddRange(
            new ReviewModel
            {
                UserId = userOneId,
                VideoGameId = games.First(g => g.Title == "Factorio").Id,
                Comment = "I really enjoyed this game!! It is super cool!",
                Rating = 5,
            },
            new ReviewModel
            {
                UserId = userTwoId,
                VideoGameId = games.First(g => g.Title == "Factorio").Id,
                Comment = "It's pretty damn good I must say so myself.!",
                Rating = 4,
            }
        );
        
        await context.SaveChangesAsync();
        
    }
}
