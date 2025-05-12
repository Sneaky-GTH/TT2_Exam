using Microsoft.EntityFrameworkCore;
using TT2_Exam.Models;

namespace TT2_Exam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<VideoGameModel> VideoGames { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship
            modelBuilder.Entity<VideoGameModel>()
                .HasMany(v => v.Categories)
                .WithMany(c => c.VideoGames)
                .UsingEntity(j => j.ToTable("VideoGameCategories"));
        }

    }
}