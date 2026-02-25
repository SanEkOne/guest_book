using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Services;
using System.Security.Cryptography;
using System.Text;

namespace mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Login,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.RegisterAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model)
        {
            var user = await _userService.AuthenticateAsync(model.Login, model.Password);

            if (user != null)
            {
                var option = new CookieOptions { Expires = DateTime.Now.AddDays(10) };
                Response.Cookies.Append("login", user.Login, option);
                return RedirectToAction("Index", "Message");
            }

            ModelState.AddModelError("", "Неверный логин или пароль!");
            return View(model);
        }

        public ActionResult Logout()
        {
            Response.Cookies.Delete("login"); 
            return RedirectToAction("Index", "Message");
        }

    }
}
