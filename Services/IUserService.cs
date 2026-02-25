namespace mvc.Services
{
    public interface IUserService
    {
        Task RegisterAsync(User user);
        Task<User?> AuthenticateAsync(string login, string password);
    }
}
