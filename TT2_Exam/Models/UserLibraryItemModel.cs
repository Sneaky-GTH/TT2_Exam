using System.ComponentModel.DataAnnotations;

namespace TT2_Exam.Models;

public class UserLibraryItemModel
{
    [MaxLength(255)] public string? UserId { get; set; }
    public UserModel? User { get; set; }

    public int VideoGameId { get; set; }
    public VideoGameModel? VideoGame { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
}