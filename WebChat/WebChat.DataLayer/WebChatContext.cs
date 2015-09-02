namespace WebChat.DataLayer
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Contracts;
    using Models;
    public class WebChatContext : IdentityDbContext<ApplicationUser>, IWebChatContext
    {
        public WebChatContext()
            : base("name=WebChatContext")
        {
            Database.SetInitializer(
                  new DropCreateDatabaseAlways<WebChatContext>());
        }

        public IDbSet<Tag> Tags { get; set; }

        public IDbSet<Room> Rooms { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

        public IDbSet<UserSession> UserSessions { get; set; }

        public static WebChatContext Create()
        {
            return new WebChatContext();
        }

    }
}
