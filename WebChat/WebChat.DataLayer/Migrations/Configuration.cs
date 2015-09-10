using System.Linq;
using WebChat.Models;

namespace WebChat.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<WebChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "WebChat.Data.WebChatContext";
        }

        protected override void Seed(WebChatContext context)
        {
            if (!context.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "asdasd",
                    Email = "asdasd@asdasd",
                    PasswordHash = "ABOYgM+IChRPgbaPEHn+7+4xWX5fRptDfktaFOkAFNEXrwkcryiU19UwqjJiZj+IBw==",
                    SecurityStamp = "fea5e506-178b-4674-84fb-6913845271c6"
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

           
        }
    }
}
