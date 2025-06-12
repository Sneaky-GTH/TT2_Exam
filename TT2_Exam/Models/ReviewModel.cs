using System.ComponentModel.DataAnnotations;

namespace TT2_Exam.Models;

public class ReviewModel
{
    public int Id { get; set; }
    [MaxLength(255)] public required string UserId { get; set; }
    public UserModel? User { get; set; }

    [Required]
    public required int VideoGameId { get; set; }
    public VideoGameModel? VideoGame { get; set; }

    [Required]
    [Range(1, 10)]
    public int Rating { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}