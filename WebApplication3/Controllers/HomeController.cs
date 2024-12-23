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

            // Khởi tạo ResourceManager, chỉ định đường dẫn tệp tài nguyên
            _resourceManager = new ResourceManager("WebApplication3.Resources.HomeController", typeof(HomeController).Assembly);
        }

        public IActionResult Index()
        {
            var culture = HttpContext.Features.Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>()?.RequestCulture.Culture
                 ?? CultureInfo.CurrentUICulture;

            ViewData["WelcomeMessage"] = _resourceManager.GetString("WelcomeMessage", culture);
            ViewData["HomePageDescription"] = _resourceManager.GetString("HomePageDescription", culture);

            return View();
        }

        public IActionResult Privacy()
        {
            // Tương tự, lấy văn hóa hiện tại và chuỗi PrivacyMessage
            var culture = HttpContext.Features.Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>()?.RequestCulture.Culture
                          ?? CultureInfo.CurrentUICulture;

            ViewData["PrivacyMessage"] = _resourceManager.GetString("PrivacyMessage", culture);
            return View();
        }
    }
}
