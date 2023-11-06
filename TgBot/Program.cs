using Data.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json.Serialization;
using Service.Abstract;
using Service.Core;
using Service.Core.TelegramBot;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация
builder.Configuration.AddJsonFile("appsettings.json");

// Конфигурация сервисов
builder.Services.AddDbContext<ApplicationDbContext>(options => { /* Не добавляем конфигурацию здесь */ });
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IMobileManager, MobileManager>();
builder.Services.AddTransient<ISettingsManager, SettingsManager>();
builder.Services.AddTransient<ICustomerManager, CustomerManager>();

// Регистрация AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Телеграм бот
var telegramBot = new TelegramBotManager();
var updateDistributor = new UpdateDistributor();
builder.Services.AddSingleton(updateDistributor);
//NOTE: регистрация телеграм менеджера в DI для лаконичности
builder.Services.AddSingleton(telegramBot);

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(6);
    options.SlidingExpiration = true;
    options.LoginPath = "/Account/Login";
});

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = null // Установите в null для использования оригинальных имен свойств
        };
    });

builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
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

app.MapControllers();

app.Run();