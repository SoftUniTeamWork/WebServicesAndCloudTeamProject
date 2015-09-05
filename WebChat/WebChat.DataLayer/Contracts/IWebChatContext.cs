using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebChat.Models;

namespace WebChat.DataLayer.Contracts
{
    public interface IWebChatContext
    {
        IDbSet<UserSession> UserSessions { get; set; }

        IDbSet<Tag> Tags { get; set; }

        IDbSet<Room> Rooms { get; set; }

        IDbSet<Message> Messages { get; set; }

        IDbSet<Notification> Notifications { get; set; }

        IDbSet<UserRoomSession> UserRoomSessions { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}
