using Microsoft.EntityFrameworkCore;
using TT2_Exam.Models;

namespace TT2_Exam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<VideoGameModel> VideoGames { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<GameSpecificCategoryModel> GameSpecificCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }

    }
}