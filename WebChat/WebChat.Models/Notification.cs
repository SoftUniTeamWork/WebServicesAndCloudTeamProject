namespace WebChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [MinLength(10)]
        [MaxLength(50)]
        [Required]
        public string Description { get; set; }

        public DateTime HappenedAt { get; set; }

        public string Title { get; set; }

        [Required]
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}
