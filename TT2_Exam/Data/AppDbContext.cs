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
        
        public DbSet<UserLibraryItemModel> UserLibrary { get; set; }
        
        public DbSet<CartItemModel> CartItems { get; set; }
        
        public DbSet<ReviewModel> Reviews { get; set; }

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
            
            modelBuilder.Entity<UserLibraryItemModel>()
                .HasKey(ul => new { ul.UserId, ul.VideoGameId });

            modelBuilder.Entity<UserLibraryItemModel>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.Library)
                .HasForeignKey(ul => ul.UserId);

            modelBuilder.Entity<UserLibraryItemModel>()
                .HasOne(ul => ul.VideoGame)
                .WithMany(v => v.Owners)
                .HasForeignKey(ul => ul.VideoGameId);
        }

    }
}