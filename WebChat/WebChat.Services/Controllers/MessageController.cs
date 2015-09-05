using WebChat.Services.UserSessionUtilities;

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

        [SessionAuthorize]
        [HttpGet]
        public IHttpActionResult GetAllMessages(int roomId)
        {
            var room = this.Data.Rooms.GetById(roomId); ;

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

        [SessionAuthorize]
        [HttpPost]
        [Route("api/rooms/{roomid}/messages")]
        public IHttpActionResult CreateMessage(int roomId, CreateMessageBindingModels model)
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.GetAll().FirstOrDefault(u => u.Id == userId);
            var room = this.Data.Rooms.GetById(roomId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            if (this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid input model");
            }

            if (room == null)
            {
                return this.BadRequest("Room doesn't exist");
            }

            var message = new Message()
            {
                Text = model.Text,
                SentDate = DateTime.Now,
                Poster = user,
                Room = room
            };

            this.Data.Messages.Add(message);
            this.Data.SaveChanges();

            return this.Ok("Message successfully created");
        }

        
    }
}