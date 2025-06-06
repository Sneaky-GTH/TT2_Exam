namespace TT2_Exam.Models;

public class GameSpecificCategoryModel
{
    public int VideoGameId { get; set; }
    public VideoGameModel? VideoGame { get; set; }

    public int CategoryId { get; set; }
    public CategoryModel? Category { get; set; }
}