using Microsoft.EntityFrameworkCore;
using TT2_Exam.Models;  // Replace with your namespace for models

namespace TT2_Exam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        //public DbSet<VideoGameModel> VideoGames { get; set; }  // Add your DbSet for models
        // Add other DbSets here as needed
    }
}