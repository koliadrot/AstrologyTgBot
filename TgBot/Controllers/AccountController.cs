namespace TgBot.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service.Abstract;
    using Service.ViewModels;
    using System.Security.Claims;

    public class AccountController : Controller
    {
        private readonly IMobileManager _mobileManager;
        private readonly IUserManager _userMager;
        public AccountController(IMobileManager mobileManager, IUserManager userMager)
        {
            _mobileManager = mobileManager;
            _userMager = userMager;
        }

        public IActionResult Index() => View();

        public IActionResult Login(string? returnUrl)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                ViewData["Message"] = "Your application description page.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Here we must validate a password
            var user = await _mobileManager.Auth(model);
            if (user != null)
            {
                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    //Whether the authentication session is persisted across
                    // multiple requests.When used with cookies,
                    //controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("FullName", user.Name),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties);

                //_logger.LogInformation("User {Email} logged in at {Time}.",
                //    user.Email, DateTime.UtcNow);

                return LocalRedirect(ReturnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при попытке входа в систему.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> LogOff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        private ActionResult RedirectToLocal(string returnUrl) => Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("TelegramBotParams", "Home");
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error() => View();
    }
}
