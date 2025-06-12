namespace TT2_Exam.Models;

public class CartItemModel
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public UserModel User { get; set; } = null!;

    public int VideoGameId { get; set; }
    public VideoGameModel VideoGame { get; set; } = null!;

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
}