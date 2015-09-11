using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Services.Models.ViewModels
{
    public class MessageOutputModel
    {
        public DateTime SentDate { get; set; }

        public string PosterId { get; set; }

        public string PosterName { get; set; }

        public int Id { get; set; }

        public string Text { get; set; }

        public int RoomId { get; set; }
    }
}