using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using TT2_Exam.Models;

namespace TT2_Exam.Utility;

public class RevalidateUserClaims(UserManager<UserModel> userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var userId = userManager.GetUserId(principal);
        if (userId != null)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
        }

        return principal;
    }
}