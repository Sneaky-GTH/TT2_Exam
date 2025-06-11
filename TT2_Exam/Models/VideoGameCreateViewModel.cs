using System.ComponentModel.DataAnnotations;

namespace TT2_Exam.Models;

public class VideoGameCreateViewModel
{
    public VideoGameModel? VideoGame { get; set; }
    
    public List<CategoryCheckboxViewModel>? Categories { get; set; }
    
    [Display(Name = "Thumbnail Image")]
    public IFormFile? ThumbnailUpload { get; set; }
}