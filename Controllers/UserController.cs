using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly MessageContext _context;

        public UserController(MessageContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Login,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);

            if (user != null)
            {
                var option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Append("login", model.Login, option);

                return RedirectToAction("Index", "Message");
            }
                

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(model);
        }

        public ActionResult Logout()
        {
            Response.Cookies.Delete("login"); 
            return RedirectToAction("Index", "Message");
        }

    }
}
