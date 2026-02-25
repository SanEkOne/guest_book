using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc;
using mvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _messageService.GetAllMessagesAsync();
            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Text")] Message message)
        {
            if (!ModelState.IsValid)
            {
                return View(await _messageService.GetAllMessagesAsync());
            }

            string? userLogin = Request.Cookies["login"];
            if (string.IsNullOrEmpty(userLogin))
            {
                return RedirectToAction("Index", "User");
            }

            try
            {
                await _messageService.CreateMessageAsync(message.Text, userLogin);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }

    }
}
