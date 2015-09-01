namespace WebChat.DataLayer
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Contracts;

    public class WebChatContext : IdentityDbContext<ApplicationUser>, IWebChatContext
    {
        public WebChatContext()
            : base("name=WebChatContext")
        {
            Database.SetInitializer(
                  new DropCreateDatabaseAlways<WebChatContext>());
        }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

        public static WebChatContext Create()
        {
            return new WebChatContext();
        }

    }
}
