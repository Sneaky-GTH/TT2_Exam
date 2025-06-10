namespace TT2_Exam.Models;

public class UserLibraryItem
{
    public string UserId { get; set; }
    public UserModel? User { get; set; }

    public int VideoGameId { get; set; }
    public VideoGameModel? VideoGame { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
}