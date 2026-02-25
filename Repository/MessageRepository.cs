using Microsoft.EntityFrameworkCore;

namespace mvc.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageContext _context;

        public MessageRepository(MessageContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetAllWithUsersAsync()
        {
            return await _context.Messages
                .Include(m => m.User)
                .OrderByDescending(m => m.DateTime)
                .ToListAsync();
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
