using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Resources;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AccountController : Controller
    {
        private readonly ResourceManager _resourceManager;

        public AccountController()
        {
            // Khởi tạo ResourceManager. Đảm bảo namespace và đường dẫn tài nguyên chính xác.
            _resourceManager = new ResourceManager("WebApplication3.Resources.Resources", typeof(AccountController).Assembly);
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            // Truyền dữ liệu đa ngôn ngữ vào ViewData
            ViewData["Login"] = _resourceManager.GetString("Login", CultureInfo.CurrentUICulture);
            ViewData["Username"] = _resourceManager.GetString("Username", CultureInfo.CurrentUICulture);
            ViewData["Password"] = _resourceManager.GetString("Password", CultureInfo.CurrentUICulture);
            ViewData["Submit"] = _resourceManager.GetString("Submit", CultureInfo.CurrentUICulture);

            // Hiển thị thông báo nếu có
            if (TempData["Message"] != null)
            {
                ViewData["Message"] = TempData["Message"];
            }

            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                // Lưu thông tin đăng nhập vào Session
                HttpContext.Session.SetString("Username", username);

                // Chuyển hướng đến trang chủ
                return RedirectToAction("Index", "Home");
            }

            ViewData["Error"] = "Invalid username or password.";
            return View();
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            // Truyền dữ liệu đa ngôn ngữ vào ViewData
            ViewData["Register"] = _resourceManager.GetString("Register", CultureInfo.CurrentUICulture);
            ViewData["Username"] = _resourceManager.GetString("Username", CultureInfo.CurrentUICulture);
            ViewData["Password"] = _resourceManager.GetString("Password", CultureInfo.CurrentUICulture);
            ViewData["Email"] = _resourceManager.GetString("Email", CultureInfo.CurrentUICulture);
            ViewData["Submit"] = _resourceManager.GetString("Submit", CultureInfo.CurrentUICulture);

            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                ViewData["Error"] = "Please fill in all required fields.";
                return View();
            }

            // Lưu thông tin vào database
            var user = new User
            {
                Username = username,
                Password = password,
                Email = email
            };

            using (var context = new AppDbContext(new DbContextOptions<AppDbContext>()))
            {
                context.Users.Add(user);
                context.SaveChanges(); // Lưu dữ liệu vào database
            }

            TempData["Message"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Xóa thông tin trong Session
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

    }
}
