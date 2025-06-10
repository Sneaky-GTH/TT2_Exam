using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Models;

namespace TT2_Exam.Data
{
    public class AppDbContext : IdentityDbContext<UserModel>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<VideoGameModel> VideoGames { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<GameSpecificCategoryModel> GameSpecificCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<GameSpecificCategoryModel>()
                .HasKey(vc => new { vc.VideoGameId, vc.CategoryId });

            modelBuilder.Entity<GameSpecificCategoryModel>()
                .HasOne(vc => vc.VideoGame)
                .WithMany(v => v.GameSpecificCategories)
                .HasForeignKey(vc => vc.VideoGameId);

            modelBuilder.Entity<GameSpecificCategoryModel>()
                .HasOne(vc => vc.Category)
                .WithMany(c => c.GameSpecificCategories)
                .HasForeignKey(vc => vc.CategoryId);
            
            modelBuilder.Entity<VideoGameModel>()
                .HasOne(v => v.Publisher)
                .WithMany(u => u.PublishedGames)
                .HasForeignKey(v => v.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<UserLibraryItem>()
                .HasKey(ul => new { ul.UserId, ul.VideoGameId });

            modelBuilder.Entity<UserLibraryItem>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.Library)
                .HasForeignKey(ul => ul.UserId);

            modelBuilder.Entity<UserLibraryItem>()
                .HasOne(ul => ul.VideoGame)
                .WithMany(v => v.Owners)
                .HasForeignKey(ul => ul.VideoGameId);
        }

    }
}