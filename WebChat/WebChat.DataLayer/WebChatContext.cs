using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WebChat.DataLayer.Migrations;
using WebChat.Models;

namespace WebChat.DataLayer
{
    public class WebChatContext : IdentityDbContext<ApplicationUser>
    {
        public WebChatContext()
            : base("name=WebChatContext")
        {
            Database.SetInitializer(
                  new MigrateDatabaseToLatestVersion<WebChatContext,
                      Configuration>());
        }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public static WebChatContext Create()
        {
            return new WebChatContext();
        }

    }
}
