namespace TgBot.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service.Abstract;
    using Service.Core.TelegramBot;
    using Service.ViewModels;
    using Service.ViewModels.TelegramModels;
    using System.Diagnostics;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserManager _userManager;
        private readonly ISettingsManager _settingsManager;
        private readonly TelegramBotManager _telegramBotManager;
        private readonly UpdateDistributor _updateDistributor;

        public HomeController(ILogger<HomeController> logger, IUserManager userManager, ISettingsManager settingsManager, TelegramBotManager telegramBotManager, UpdateDistributor updateDistributor, ICustomerManager customerManager)
        {
            _logger = logger;
            _userManager = userManager;
            _settingsManager = settingsManager;
            _telegramBotManager = telegramBotManager;
            _updateDistributor = updateDistributor;
            _updateDistributor.Init(customerManager, settingsManager, telegramBotManager);
        }

        public IActionResult Index() => RedirectToAction("TelegramBotParams", "Home");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        public async Task<ActionResult> TelegramBotParams()
        {
            TelegramBotParamsViewModel viewModel = _settingsManager.GetTelegramBot();

            ViewBag.IsStarted = _telegramBotManager.IsStarted;
            //ViewBag.IsSuperAdmin = _accessManager.CheckUserPermission(User.Identity.Name, nameof(AdminController));
            ViewBag.IsSuperAdmin = true;
            return View(viewModel);
        }

        public ActionResult TelegramBotCostructorMenu()
        {
            TelegramBotParamsViewModel viewModel = _settingsManager.GetTelegramBot();
            return View(viewModel);
        }

        public ActionResult TelegramBotMenu()
        {
            TelegramBotParamsViewModel viewModel = _settingsManager.GetTelegramBot();
            //ViewBag.IsSuperAdmin = _accessManager.CheckUserPermission(User.Identity.Name, nameof(AdminController));
            ViewBag.IsSuperAdmin = true;
            return View(viewModel);
        }

        public ActionResult TelegramBotRegisterConsitions()
        {
            TelegramBotParamsViewModel viewModel = _settingsManager.GetTelegramBot();
            //ViewBag.IsSuperAdmin = _accessManager.CheckUserPermission(User.Identity.Name, nameof(AdminController));
            ViewBag.IsSuperAdmin = true;
            return View(viewModel);
        }

        public ActionResult TelegramBotParamMessages()
        {
            TelegramBotParamsViewModel viewModel = _settingsManager.GetTelegramBot();
            return View(viewModel);
        }
    }
}
