using ComputerStoreContracts.Services;
using ComputerStoreModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ComputerStoreWebStorekeeper.Controllers
{
    public class AuthController : Controller
    {
        private readonly IStorekeeperService _storekeeperService;

        public AuthController(IStorekeeperService storekeeperService)
        {
            _storekeeperService = storekeeperService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = await _storekeeperService.LoginAsync(login, password);
            if (user == null)
                return View("Error");

            // Сохраняем в сессию
            HttpContext.Session.SetString("User", JsonSerializer.Serialize(user));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            user.UserType = ComputerStoreModels.Enums.UserType.Storekeeper;
            await _storekeeperService.RegisterAsync(user);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login");
        }
    }
}
