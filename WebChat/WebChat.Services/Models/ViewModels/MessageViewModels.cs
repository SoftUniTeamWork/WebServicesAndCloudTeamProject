using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebChat.Models;

namespace WebChat.Services.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        public DateTime SentDate { get; set; }

        public string Text { get; set; }

        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        public string PosterId { get; set; }

        public virtual ApplicationUser Poster { get; set; }

        public static Expression<Func<Message, MessageViewModel>> Create
        {
            get
            {
                return m => new MessageViewModel()
                {
                    Id = m.Id,
                    SentDate = m.SentDate,
                    Text = m.Text,
                    PosterId = m.PosterId,
                    RoomId = m.RoomId,
                };
            }
        } 
    }
}