using System.ComponentModel.DataAnnotations;

namespace TT2_Exam.Models;

public class ReviewCreateViewModel
{
    [Required]
    public int VideoGameId { get; set; }

    [Required]
    [Range(1, 10)]
    public int Rating { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;
}