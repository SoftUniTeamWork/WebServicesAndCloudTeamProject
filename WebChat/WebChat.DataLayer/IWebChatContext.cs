using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebChat.Models;

namespace WebChat.DataLayer
{
    public interface IWebChatContext
    {
        DbSet<Tag> Tags { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Notification> Notifications { get; set; }
    }
}
