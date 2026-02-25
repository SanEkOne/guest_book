using Microsoft.EntityFrameworkCore;
using mvc.Repository;

namespace mvc.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepo;

        public MessageService(IMessageRepository messageRepo, IUserRepository userRepo)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _messageRepo.GetAllWithUsersAsync();
        }

        public async Task CreateMessageAsync(string text, string userLogin)
        {
            var user = await _userRepo.GetByLoginAsync(userLogin);
            if (user == null) throw new Exception("User not found");

            var message = new Message 
            { 
                Text = text, 
                UserId = user.Id, 
                DateTime = DateTime.Now 
            };

            await _messageRepo.AddAsync(message);
            await _messageRepo.SaveChangesAsync();
        }
    }
}
