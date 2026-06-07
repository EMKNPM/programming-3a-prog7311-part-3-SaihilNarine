using Microsoft.AspNetCore.Mvc;
using PROG7311_POE_.Models;
using PROG7311_POE_.Services;

namespace PROG7311_POE_.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthenticationApiService _authService;

        public AccountController(AuthenticationApiService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = await _authService.LoginAsync(
                model.Username,
                model.Password);

            if (token == null)
            {
                ModelState.AddModelError("", "Invalid login");
                return View(model);
            }

            HttpContext.Session.SetString("JWT", token);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}