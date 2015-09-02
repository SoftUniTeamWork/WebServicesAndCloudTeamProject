using Microsoft.AspNet.Identity.EntityFramework;
using WebChat.Models;

namespace WebChat.DataLayer.Contracts
{
    public interface IWebChatData
    {
        IGenericRepository<ApplicationUser> Users { get; }

        IGenericRepository<IdentityRole> UserRoles { get; }

        IGenericRepository<Room> Rooms { get; }

        IGenericRepository<Notification> Notifications { get; }

        IGenericRepository<Message> Messages { get; }

        IGenericRepository<UserSession> UserSessions { get; }

        IGenericRepository<Tag> Tags { get; } 

        int SaveChanges();
    }
}
