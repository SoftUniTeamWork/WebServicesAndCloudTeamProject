namespace WebChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

        public string Password { get; set; }

        public RoomType Type { get; set; }

        [DefaultValue(20)]
        [Range(0,50)]
        public int Size { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public virtual ICollection<Tag> Tags { get; set; }
        
        [Required]
        [ForeignKey("ApplicationUser")]
        public int OwnerId { get; set; }

        [Required]
        public virtual ApplicationUser Owner { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<Notification> Notifications { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

    }
}
