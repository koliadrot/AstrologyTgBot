using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json.Serialization;
using NLog;
using Service.Abstract;
using Service.Core;
using Service.Core.TelegramBot;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IMobileManager, MobileManager>();
builder.Services.AddTransient<ISettingsManager, SettingsManager>();
builder.Services.AddTransient<ICustomerManager, CustomerManager>();
NLog.ILogger logger = LogManager.GetLogger("default");
builder.Services.AddSingleton(logger);

// Регистрация AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Телеграм бот
var updateDistributor = new UpdateDistributor();
var telegramBot = new TelegramBotManager();
telegramBot.Logger = logger;
telegramBot.Start();

//NOTE: регистрация телеграм менеджера в DI для лаконичности
builder.Services.AddSingleton(updateDistributor);
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