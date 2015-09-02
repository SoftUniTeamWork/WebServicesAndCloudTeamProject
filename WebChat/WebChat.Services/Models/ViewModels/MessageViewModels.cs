using System;
using System.Linq.Expressions;
using WebChat.Models;

namespace WebChat.Services.Models.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        public DateTime SentDate { get; set; }

        public string Text { get; set; }

        public string PosterId { get; set; }

        public static Expression<Func<Message, MessageViewModel>> Create
        {
            get
            {
                return m => new MessageViewModel()
                {
                    Id = m.Id,
                    SentDate = m.SentDate,
                    Text = m.Text,
                    PosterId = m.Poster.Id
                };
            }
        } 
    }
}