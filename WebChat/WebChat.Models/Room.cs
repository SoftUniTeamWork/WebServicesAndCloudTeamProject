using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebChat.Models
{
    public class Room
    {
        public Room()
        {
            this.Notifications = new HashSet<Notification>();
            this.Users = new HashSet<ApplicationUser>();
            this.Tags = new HashSet<Tag>();
            this.Messages = new HashSet<Message>();
            this.Size = 20;
        }
        
        public int Id { get; set; }

        public string Password { get; set; }

        [DefaultValue(RoomType.Private)]
        public RoomType Type { get; set; }

        [DefaultValue(20)]
        [Range(0,50)]
        public int Size { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}
