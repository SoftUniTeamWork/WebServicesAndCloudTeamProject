namespace WebChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Room
    {
        private ICollection<Tag> tags;
        private ICollection<Notification> notifications;
        private ICollection<Message> messages;
        private ICollection<ApplicationUser> users;

        public Room()
        {
            this.notifications = new HashSet<Notification>();
            this.users = new HashSet<ApplicationUser>();
            this.tags = new HashSet<Tag>();
            this.messages = new HashSet<Message>();
            this.Size = 20;
        }
        
        [Key]
        public int Id { get; set; }

        public string Password { get; set; }

        public RoomType Type { get; set; }

        [Range(0,50)]
        public int Size { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public virtual ICollection<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        public virtual ICollection<Message> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }

        public virtual ICollection<Notification> Notifications
        {
            get { return this.notifications; }
            set { this.notifications = value; }
        }

        public virtual ICollection<ApplicationUser> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }
    }
}
