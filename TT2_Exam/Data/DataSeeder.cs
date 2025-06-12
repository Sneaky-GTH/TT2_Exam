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
            new VideoGameModel
            {
                Title = "Hi-Fi Rush", PublisherId = publisherOneId, Price = 39.99M,
                ShortDescription = "Beat up robots to the rhythm!", ThumbnailPath = "~/img/hifirush_thumbnail.jpg",
                Description = "**Hi-Fi RUSH** is a rhythm-based action game where every move you make syncs with the beat. *Chai*, a wannabe rockstar, finds himself with a music player fused to his heart and must now fight an evil megacorp to the rhythm of the music.\n\nBattle through colorful levels, string combos in time with high-energy tracks, and face off against powerful bosses. With vibrant visuals, rhythm-infused mechanics, and a cast of eccentric allies, *Hi-Fi RUSH* is a thrilling ride for music and action lovers."
            },
            new VideoGameModel
            {
                Title = "Cyberpunk 2077", PublisherId = publisherOneId, Price = 49.99M,
                ShortDescription = "Explore Night City - become a legend.", ThumbnailPath = "~/img/cyberpunk_thumbnail.jpg",
                Description = "**Cyberpunk 2077** is an open-world, action-adventure RPG set in *Night City*, a megalopolis obsessed with power, glamour, and body modification. You play as *V*, a mercenary outlaw going after a one-of-a-kind implant that is the key to immortality.\n\nCustomize your character’s cyberware, skillset, and playstyle, and explore a vast city where your choices shape the story and the world around you. With dynamic gameplay and a deep narrative, experience a gritty future where tech and humanity collide."
            },
            new VideoGameModel
            {
                Title = "Yakuza 0", PublisherId = publisherOneId, Price = 19.99M,
                ShortDescription = "The glitz, glamour, and unbridled decadence of the 80s are back in Yakuza 0.", ThumbnailPath = "~/img/yakuza0_thumbnail.png",
                Description = "**Yakuza 0** takes you back to the roots of the *Yakuza* series in 1980s Japan. Play as *Kazuma Kiryu* and *Goro Majima* in a gripping crime drama filled with brutal combat, quirky side stories, and unforgettable characters.\n\nFrom intense brawls to managing hostess clubs, explore the neon-soaked streets of Tokyo and Osaka. Experience a story of loyalty, betrayal, and honor in a city overflowing with distractions and danger."
            },
            new VideoGameModel
            {
                Title = "Fallout: New Vegas", PublisherId = publisherOneId, Price = 9.99M,
                ShortDescription = "Explore post-apocalyptic America and choose the fate of New Vegas.", ThumbnailPath = "~/img/fnv_thumbnail.jpg",
                Description = "**Fallout: New Vegas** drops you into a post-apocalyptic Mojave Wasteland where factions vie for control of *New Vegas*. As the *Courier*, you’re left for dead and must uncover a conspiracy that could change the fate of the region.\n\nWith deep RPG mechanics, branching storylines, and the freedom to shape your destiny, *New Vegas* offers one of the most immersive open-world experiences in gaming. Will you choose peace, war, or something in between?"
            },
            new VideoGameModel
            {
                Title = "Hades", PublisherId = publisherOneId, Price = 24.99M,
                ShortDescription = "Fight. Die. Repeat. Reach the Surface.", ThumbnailPath = "~/img/hades_thumbnail.jpeg",
                Description = "**Hades** is a rogue-like dungeon crawler where you play as *Zagreus*, the son of Hades, attempting to escape the Underworld. With help from Olympian gods and powerful boons, you’ll battle through shifting levels of mythic chaos.\n\nEach run is different, blending fast-paced combat with a rich narrative and stunning art. Death is only the beginning, as every failure makes you stronger and brings new revelations."
            },
            new VideoGameModel
            {
                Title = "Balatro", PublisherId = publisherTwoId, Price = 9.99M,
                ShortDescription = "A poker roguelike.", ThumbnailPath = "~/img/balatro_thumbnail.png",
                Description = "**Balatro** is a unique *poker roguelike* that mixes deck-building mechanics with the unpredictability of classic poker hands. Chain hands together, use Joker cards to manipulate outcomes, and push your luck for powerful combos.\n\nEvery run presents new challenges and opportunities for strategic thinking. Whether you're a poker master or a strategy novice, *Balatro* delivers satisfying depth and addictive gameplay."
            },
            new VideoGameModel
            {
                Title = "HELLDIVERS™ 2", PublisherId = publisherTwoId, Price = 39.99M,
                ShortDescription = "For Super-Earth!", ThumbnailPath = "~/img/hd2_thumbnail.jpg",
                Description = "**HELLDIVERS™ 2** is an intense third-person co-op shooter where you fight for the survival of *Super-Earth*. Gear up and drop into hostile territory with your squad to take on massive alien threats.\n\nCoordinate tactics, call in devastating airstrikes, and avoid friendly fire as you fight for democracy across the galaxy. With deep customization and chaotic fun, *HELLDIVERS™ 2* rewards teamwork and precision."
            },
            new VideoGameModel
            {
                Title = "Hades II", PublisherId = publisherTwoId, Price = 24.99M,
                ShortDescription = "The sequel to 'Hades' is finally here!", ThumbnailPath = "~/img/hades2_thumbnail.jpg",
                Description = "**Hades II** builds on the award-winning rogue-like formula of its predecessor. Take on the role of *Melinoë*, the Princess of the Underworld, as she battles to defeat the Titan of Time, *Chronos*.\n\nWield powerful magic, uncover new boons from Olympian gods, and navigate through a reimagined Greek mythos. With fluid combat, deep progression, and ever-evolving storytelling, *Hades II* elevates every aspect of the original game."
            },
            new VideoGameModel
            {
                Title = "Disco Elysium", PublisherId = publisherTwoId, Price = 26.95M,
                ShortDescription = "Be a detective and solve the case, all while trying not to die from alcoholism.", ThumbnailPath = "~/img/discoelysium_thumbnail.jpg",
                Description = "**Disco Elysium** is a groundbreaking RPG where you play as a troubled detective trying to solve a murder in a decaying city. With no traditional combat, your choices and dialogue define your skills, mindset, and fate.\n\nExplore a richly written world filled with philosophical depth, moral ambiguity, and unforgettable characters. Whether you become a hero, a disaster, or something in between, *Disco Elysium* is a masterclass in interactive storytelling."
            },
            new VideoGameModel
            {
                Title = "Nine Sols", PublisherId = publisherTwoId, Price = 19.99M,
                ShortDescription = "A 2D action platformer set in a Far Eastern cyberpunk world.", ThumbnailPath = "~/img/ninesols_thumbnail.jpg",
                Description = "**Nine Sols** is a 2D action-platformer set in a *Taopunk* world blending cyberpunk and East Asian mythology. You are *Yi*, a vengeful hero on a quest to destroy the 9 Sols—powerful beings who once ruled the land.\n\nEngage in fast-paced, Sekiro-inspired deflection-based combat and uncover the secrets of a mysterious world. With hand-drawn visuals and deep lore, *Nine Sols* is as artistic as it is challenging."
            },
            new VideoGameModel
            {
                Title = "GUILTY GEAR -STRIVE-", PublisherId = publisherTwoId, Price = 29.99M,
                ShortDescription = "A stylish and explosive anime fighting game.", ThumbnailPath = "~/img/ggst_thumbnail.jpg",
                Description = "**GUILTY GEAR -STRIVE-** is a cutting-edge fighting game that redefines the genre with stunning visuals, hard-hitting mechanics, and a rock-inspired aesthetic. Featuring a diverse cast of characters, each with unique abilities and playstyles, the game delivers fast and fluid combat.\n\nNew systems like *Wall Break* and *Roman Cancels* deepen the strategic options, while an accessible control scheme opens the door for newcomers. Whether you're a veteran or a fresh fighter, *Strive* hits hard and looks amazing doing it."
            },
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
