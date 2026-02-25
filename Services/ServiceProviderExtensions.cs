using System.Diagnostics.Metrics;

namespace mvc.Services
{
    public static class ServiceProviderExtensions
    {
        public static void AddUserService(this IServiceCollection services)
        {
            services.AddTransient<IUserService>();
            services.AddTransient<UserService>();
        }
    }
}
