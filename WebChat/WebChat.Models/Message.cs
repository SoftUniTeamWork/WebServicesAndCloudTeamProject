namespace WebChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message
    {
        public int Id { get; set; }
        [Required]
        public DateTime SentDate { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        [Required]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [Required]
        public string PosterId { get; set; }

        public virtual ApplicationUser Poster { get; set; }
    }
}
