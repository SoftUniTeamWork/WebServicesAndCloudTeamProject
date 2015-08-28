using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Notification
    {
        [MinLength(10)]
        [MaxLength(50)]
        [Required]
        public string Description { get; set; }

        public DateTime HappenedAt { get; set; }

        public string Title { get; set; }

        [ForeignKey("PublicRoom")]
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}
