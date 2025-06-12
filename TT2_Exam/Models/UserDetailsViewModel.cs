using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using TT2_Exam.Models;

namespace TT2_Exam.Models;

public class UserDetailsViewModel
{
    public required UserModel User { get; set; }
    public required IList<string> CurrentRoles { get; set; }
    public required List<string?> AllRoles { get; set; }
}