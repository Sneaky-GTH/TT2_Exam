using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

public class CultureController : Controller
{
    [HttpGet]
    public IActionResult SetCulture(string culture, string returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(culture))
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                }
            );
        }

        return LocalRedirect(returnUrl ?? "/");
    }
}