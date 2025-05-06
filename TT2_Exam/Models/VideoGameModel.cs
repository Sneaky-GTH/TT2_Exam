using System.ComponentModel.DataAnnotations;

namespace TT2_Exam.Models
{
    public class VideoGameModel
    {
        public int Id { get; set; }

        [Required] [MaxLength(255)] public string Title { get; set; } = string.Empty;
        
        public DateTime ReleaseDate { get; set; }
        
        public decimal Price { get; set; }
    }
}