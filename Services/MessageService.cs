using Microsoft.EntityFrameworkCore;

namespace mvc.Services
{
    public class MessageService : IMessageService
    {
        private readonly MessageContext _context;

        public MessageService(MessageContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _context.Messages
                .Include(m => m.User)
                .OrderByDescending(m => m.DateTime) 
                .ToListAsync();
        }

        public async Task CreateMessageAsync(string text, string userLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            if (user == null)
                throw new Exception("User not found"); 

            var message = new Message
            {
                Text = text,
                UserId = user.Id,
                DateTime = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
