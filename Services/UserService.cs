using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace mvc.Services
{
    public class UserService : IUserService
    {
        private readonly MessageContext _context;
        public UserService(MessageContext context)
        {
            _context = context;
        }

        public async Task RegisterAsync(User user)
        {
            byte[] saltbuf = new byte[16];
            using (var r = RandomNumberGenerator.Create())
            {
                r.GetBytes(saltbuf);
            }
            string salt = Convert.ToHexString(saltbuf);

            user.Password = HashPassword(user.Password, salt);
            user.Salt = salt;

            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null) return null;

            var hashedPassword = HashPassword(password, user.Salt);

            return user.Password == hashedPassword ? user : null;
        }

        private string HashPassword(string password, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(salt + password);
            byte[] byteHash = MD5.HashData(bytes); 
            return Convert.ToHexString(byteHash);
        }
    }
}

