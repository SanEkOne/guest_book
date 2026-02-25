namespace mvc.Services
{
    public interface IMessageService
    {
        Task<List<Message>> GetAllMessagesAsync();
        Task CreateMessageAsync(string text, string userLogin);
    }
}
