namespace TT2_Exam.Models;

public class StoreVideoGameDetailsViewModel
{
    public VideoGameModel? VideoGame { get; set; }
    
    public List<ReviewModel>? Reviews { get; set; }
    
    public bool UserOwnsGame { get; set; }
    
    public ReviewModel? UserReview { get; set; }
    
}