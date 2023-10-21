namespace TelegramBot.Controllers
{
    using Data.Core;
    using Data.Core.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using TelegramBot.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            TelegramBotParams telegramBotParam = _applicationDbContext.TelegramParams.FirstOrDefault();
            return View(telegramBotParam);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}