using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Message
    {
        [Required]
        public DateTime Sent { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        [ForeignKey("ApplicationUser")]
        public int PosterId { get; set; }

        public virtual ApplicationUser Poster { get; set; }
    }
}
