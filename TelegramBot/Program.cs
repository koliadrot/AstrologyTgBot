using Data.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service.Abstract;
using Service.Core;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация
builder.Configuration.AddJsonFile("appsettings.json");

// Конфигурация сервисов
builder.Services.AddDbContext<ApplicationDbContext>(options => { /* Не добавляем конфигурацию здесь */ });
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IMobileManager, MobileManager>();

// Регистрация AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //auth.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //auth.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(6);
    options.SlidingExpiration = true;
    options.LoginPath = "/Account/Login";
});

builder.Services.AddControllersWithViews();
builder.Services.AddKendo();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
