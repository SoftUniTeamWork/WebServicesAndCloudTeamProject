namespace WebChat.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using WebChat.Models;
    using DataLayer.Contracts;
    using DataLayer.Data;
    using Models.BindingModels;
    using Models.ViewModels;

    public class MessageController : BaseApiController
    {
        public MessageController()
            : this(new WebChatData())
        {
        }

        public MessageController(IWebChatData data)
            : base(data)
        {
        }

        [HttpGet]
        public IHttpActionResult GetAllMessages(int roomId)
        {
            var room = this.Data.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId); ;

            if (room == null)
            {
                return this.BadRequest(string.Format("Room with id {0} doesn't exist", roomId));
            }

            var messages = this.Data.Messages.GetAll()
                .Where(m => m.Room.Id == room.Id)
                .Select(MessageViewModel.Create)
                .OrderByDescending(m => m.SentDate)
                .AsQueryable();

            return this.Ok(messages);
        }

        
    }
}