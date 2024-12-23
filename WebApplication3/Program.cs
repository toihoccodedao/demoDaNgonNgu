using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApplication3.Data;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký dịch vụ Localization và View
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Thêm DbContext vào DI container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm hỗ trợ Session
builder.Services.AddDistributedMemoryCache(); // Bộ nhớ tạm cho session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout session
    options.Cookie.HttpOnly = true; // Chỉ có thể truy cập qua HTTP
    options.Cookie.IsEssential = true; // Bắt buộc cho hoạt động
});

// Danh sách các ngôn ngữ được hỗ trợ
var supportedCultures = new[]
{
    new CultureInfo("en"), // Tiếng Anh
    new CultureInfo("vi"), // Tiếng Việt
    new CultureInfo("fr"), // Tiếng Pháp
    new CultureInfo("es"), // Tiếng Tây Ban Nha
    new CultureInfo("de"), // Tiếng Đức
    new CultureInfo("it"), // Tiếng Ý
    new CultureInfo("ko") // Tiếng Hàn
};

// Cấu hình Localization
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"), // Ngôn ngữ mặc định
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

// Thêm QueryStringRequestCultureProvider vào RequestCultureProviders
localizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());

var app = builder.Build();

// Middleware Localization
app.UseRequestLocalization(localizationOptions);

// Thêm Middleware cho Session
app.UseSession();

// Middleware cơ bản
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
