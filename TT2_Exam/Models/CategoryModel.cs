using System.ComponentModel.DataAnnotations;

namespace TT2_Exam.Models;

public class CategoryModel
{
    public int Id { get; set; }
    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    public ICollection<VideoGameModel> VideoGames { get; set; } = new List<VideoGameModel>();
}