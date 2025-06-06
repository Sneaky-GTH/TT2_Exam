using static TT2_Exam.Models.VideoGameModel;

namespace TT2_Exam.Models;

public class StoreViewModel
{
    public List<VideoGameModel> Games { get; set; } = [];
    
    public List<CategoryModel> AvailableCategories { get; set; } = [];

    public List<int> SelectedCategoryIds { get; set; } = [];
    
    public string? SearchQuery { get; set; }
    
    public string? SortBy { get; set; }
}