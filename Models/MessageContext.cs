using Microsoft.EntityFrameworkCore;

namespace mvc 
{
    public class MessageContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; } = null!;
        public MessageContext(DbContextOptions<MessageContext> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Login = "Admin",
                    Password = "123admin"
                }
            );

            modelBuilder.Entity<Message>().HasData(
                new Message
                {
                    Id = 1,
                    Text = "First message!",
                    DateTime = DateTime.Now,
                    UserId = 1
                }
            );
        }

    }
}