using System.ComponentModel.DataAnnotations;

namespace TT2_Exam.Models
{
    public class VideoGameModel
    {
        public int Id { get; set; }

        [Required] [MaxLength(64)] public string Title { get; set; } = string.Empty;
        
        [MaxLength(255)] public string ShortDescription { get; set; } = string.Empty;
        
        [MaxLength(10000)] public string Description { get; set; } = string.Empty;
        
        [MaxLength(64)] public string? ThumbnailPath { get; set; } = string.Empty;
        
        public DateTime ReleaseDate { get; set; }
        
        public decimal Price { get; set; }

        [Required] public ICollection<GameSpecificCategoryModel> GameSpecificCategories { get; set; } =
            new List<GameSpecificCategoryModel>();
        
        [MaxLength(255)] public string PublisherId { get; set; } = string.Empty;
        public UserModel? Publisher { get; set; }
        
        public ICollection<UserLibraryItemModel> Owners { get; set; } = new List<UserLibraryItemModel>();

    }
}