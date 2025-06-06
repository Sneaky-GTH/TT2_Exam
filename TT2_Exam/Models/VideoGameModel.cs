using System.ComponentModel.DataAnnotations;

namespace TT2_Exam.Models
{
    public class VideoGameModel
    {
        public int Id { get; set; }

        [Required] [MaxLength(255)] public string Title { get; set; } = string.Empty;
        
        [Required] [MaxLength(2047)] public string Description { get; set; } = string.Empty;
        
        public DateTime ReleaseDate { get; set; }
        
        public decimal Price { get; set; }

        [Required] public ICollection<GameSpecificCategoryModel> GameSpecificCategories { get; set; } =
            new List<GameSpecificCategoryModel>();

    }
}