using Microsoft.EntityFrameworkCore;


namespace mvc.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MessageContext _context;

        public UserRepository(MessageContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
