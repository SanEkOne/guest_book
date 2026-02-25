namespace mvc.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByLoginAsync(string login);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
