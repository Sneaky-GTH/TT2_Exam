using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TT2_Exam.Data;
using Microsoft.EntityFrameworkCore;

namespace TT2_Exam.Utility;

public class IsPublisherHandler : AuthorizationHandler<IsPublisherRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;

    public IsPublisherHandler(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPublisherRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return;

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return;
        
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return;
        }
        
        var routeData = httpContext.GetRouteData();
        if (!routeData.Values.TryGetValue("id", out var idObj) || idObj == null)
            return;

        if (!int.TryParse(idObj.ToString(), out int videoGameId))
            return;

        // Load the video game with publisher
        var videoGame = await _context.VideoGames
            .Include(v => v.Publisher)
            .FirstOrDefaultAsync(v => v.Id == videoGameId);

        if (videoGame == null)
            return;

        if (videoGame.PublisherId == userId)
        {
            context.Succeed(requirement);
        }
    }
}
