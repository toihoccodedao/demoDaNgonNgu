using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký dịch vụ Localization và View
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Danh sách các ngôn ngữ được hỗ trợ
var supportedCultures = new[]
{
    new CultureInfo("en"), // Tiếng Anh
    new CultureInfo("vi"), // Tiếng Việt
    new CultureInfo("fr")  // Tiếng Pháp
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

// Middleware cơ bản
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
