using static TT2_Exam.Models.VideoGameModel;

namespace TT2_Exam.Models;

public class StoreViewModel
{
    public List<VideoGameModel> Games { get; set; } = new();
    
    public List<string> AvailableCategories { get; set; } = new();
    
    public List<string> SelectedCategories { get; set; } = new();
    
    public string? SearchQuery { get; set; }
    
    public string? SortBy { get; set; }
}