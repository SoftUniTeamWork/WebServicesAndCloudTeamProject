namespace WebChat.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using WebChat.Models;
    using DataLayer.Contracts;
    using DataLayer.Data;
    using Models.BindingModels;
    using Models.ViewModels;
    using System.Web.Http.OData;
    using Providers;
    using UserSessionUtilities;
    public class MessageController : BaseApiController
    {
        private IIdProvider provider;
        public MessageController()
            : base(new WebChatData())
        {
        }

        public MessageController(IWebChatData data, IIdProvider provider)
            : base(data)
        {
            this.provider = provider;
        }

        [SessionAuthorize]
        [HttpGet]
        public IHttpActionResult GetAll(int roomId)
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
            var userId = this.provider.GetId();
            var user = this.Data.Users.GetAll().FirstOrDefault(u => u.Id == userId);
            var room = this.Data.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            if (!this.ModelState.IsValid)
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

        [HttpGet]
        [EnableQuery]
        [Route("api/rooms/{roomId}/messages")]
        public IHttpActionResult GetAllMessages(int roomId)
        {
            var room = this.Data.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                return this.BadRequest("Invalid room id");
            }

            var messages = room.Messages
                .Select(m => new MessageOutputModel()
                {
                    Id = m.Id,
                    Text = m.Text,
                    SentDate = m.SentDate,
                    PosterId = m.PosterId,
                    PosterName = m.Poster.UserName
                })
                .OrderByDescending(m => m.SentDate);

            return this.Ok(messages);
        }
        
    }
}