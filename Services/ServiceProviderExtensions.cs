using mvc.Repository;
using System.Diagnostics.Metrics;

namespace mvc.Services
{
    public static class ServiceProviderExtensions
    {
        public static void AddUserService(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageService, MessageService>();
        }
    }
}
