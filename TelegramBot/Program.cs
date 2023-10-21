using Data.Core;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация
builder.Configuration.AddJsonFile("appsettings.json");

// Конфигурация сервисов
builder.Services.AddDbContext<ApplicationDbContext>(options => { /* Не добавляем конфигурацию здесь */ });

builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();






