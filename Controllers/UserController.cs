using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
                byte[] saltbuf = new byte[16];
                var r = RandomNumberGenerator.Create(); 
                r.GetBytes(saltbuf); 

                var sb = new StringBuilder(16); 
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i])); 

                var salt = sb.ToString(); 

                byte[] password = Encoding.Unicode.GetBytes(salt + user.Password);

                var md5 = MD5.Create();

                byte[] byteHash = md5.ComputeHash(password);

                var hash = new StringBuilder(byteHash.Length); 
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Password = hash.ToString(); 
                user.Salt = salt; 

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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Login);

            if (user != null)
            {
                //string? salt = user.Salt;

                byte[] password = Encoding.Unicode.GetBytes(user.Salt + model.Password);

                var md5 = MD5.Create();

                byte[] byteHash = md5.ComputeHash(password);

                var hash = new StringBuilder(byteHash.Length);

                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Password == hash.ToString()) 
                {
                    var option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Append("login", model.Login, option);

                    return RedirectToAction("Index", "Message");
                }

                ModelState.AddModelError("", "Неверный логин или пароль!");
                return View();
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
