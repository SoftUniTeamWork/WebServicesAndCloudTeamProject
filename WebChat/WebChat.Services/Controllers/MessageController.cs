﻿using WebChat.DataLayer.Data;

namespace WebChat.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using WebChat.Models;
    using Models;
    using DataLayer.Contracts;

    public class MessageController : BaseApiController
    {
        private readonly IGenericRepository<Message> messagesRepository;
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
                return this.BadRequest(string.Format("Room with id {0} doesn't exist",roomId));
            }

            var messages = this.Data.Messages.GetAll()
                .Where(m => m.Room.Id == room.Id)
                .Select(MessageViewModel.Create)
                .OrderByDescending(m => m.SentDate)
                .AsQueryable();

            return this.Ok(messages);
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult AddMessage(int roomId, MessageBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();
            var room = this.Data.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId);
            var sendingUser = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.Id == userId);

            if (userId == null)
            {
                return this.Unauthorized();
            }
            if (model == null)
            {
                return BadRequest("Model cannot be null (no data in request)");
            }
            if (sendingUser == null)
            {
                return this.BadRequest("Invalid id of the sender");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var poster = Data.Users.GetAll().FirstOrDefault(u => u.Id == model.PosterId);

            if (poster == null)
            {
                return this.BadRequest(string.Format("User with id:{0} does not exist", poster.Id));
            }

            var message = new Message
            {
                Text = model.Text,
                Poster = poster,
                SentDate = DateTime.Now,
                Room = room
            };

            messagesRepository.Add(message);
            messagesRepository.SaveChanges();

            return Ok(string.Format("Message from user with id = {0} successfully sent!", poster.Id));
        }
    }
}