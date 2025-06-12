using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TT2_Exam.Models

{
    public class UserModel : IdentityUser
    {
        [Required] [MaxLength(64)] public string DisplayName { get; set; } = string.Empty;
        
        public ICollection<VideoGameModel> PublishedGames { get; set; } = new List<VideoGameModel>();
        
        public ICollection<UserLibraryItemModel> Library { get; set; } = new List<UserLibraryItemModel>();
    }
}