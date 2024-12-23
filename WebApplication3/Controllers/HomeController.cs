using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Resources;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ResourceManager _resourceManager;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            // Đường dẫn chính xác đến ResourceManager
            _resourceManager = new ResourceManager("WebApplication3.Resources.HomeController", typeof(HomeController).Assembly);
        }

        public IActionResult Index()
        {
            var culture = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture
                          ?? CultureInfo.CurrentUICulture;

            ViewData["WelcomeMessage"] = _resourceManager.GetString("WelcomeMessage", culture) ?? "Default Welcome Message";
            ViewData["HomePageDescription"] = _resourceManager.GetString("HomePageDescription", culture) ?? "Default Description";
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("Username") != null;
            return View();         
        }

        public IActionResult Privacy()
        {
            var culture = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture
                          ?? CultureInfo.CurrentUICulture;

            ViewData["PrivacyMessage"] = _resourceManager.GetString("PrivacyMessage", culture) ?? "Default Privacy Message";

            return View();
        }

        [HttpPost]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
