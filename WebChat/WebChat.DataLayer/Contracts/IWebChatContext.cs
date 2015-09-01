using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebChat.Models;

namespace WebChat.DataLayer.Contracts
{
    public interface IWebChatContext
    {
        DbSet<Tag> Tags { get; set; }

        DbSet<Room> Rooms { get; set; }

        DbSet<Message> Messages { get; set; }

        DbSet<Notification> Notifications { get; set; }

        DbSet<ApplicationUser> Users { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}
