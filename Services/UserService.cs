using Microsoft.EntityFrameworkCore;
using mvc.Repository;
using System.Security.Cryptography;
using System.Text;

namespace mvc.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            var user = await _userRepository.GetByLoginAsync(login);
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

