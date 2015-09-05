using System;

namespace WebChat.Models
{
    public class UserRoomSession
    {
        public int Id { get; set; }

        public DateTime JoinDate { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User{ get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }
    }
}
