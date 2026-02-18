using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc;

namespace mvc.Controllers
{
    public class MessageController : Controller
    {
        private readonly MessageContext _context;

        public MessageController(MessageContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.Include(m => m.User).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Text")] Message message)
        {
            if (!ModelState.IsValid)
                return View(await _context.Messages.Include(m => m.User).ToListAsync());

            string? userLogin = Request.Cookies["login"];
            if (string.IsNullOrEmpty(userLogin))
            {
                return RedirectToAction("index", "User");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin);
            if (user == null)
            {
                return RedirectToAction("User", "Login");
            }

            message.UserId = user.Id;
            message.DateTime = DateTime.Now;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
