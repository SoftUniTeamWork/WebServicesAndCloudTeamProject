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
            
        }
    }
}
