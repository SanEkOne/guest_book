namespace mvc.Repository
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllWithUsersAsync();
        Task AddAsync(Message message);
        Task SaveChangesAsync();
    }
}
