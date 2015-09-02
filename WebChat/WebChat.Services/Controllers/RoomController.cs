namespace WebChat.Services.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using DataLayer.Contracts;
    using DataLayer.Data;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.Utilities;
    using Models.ViewModels;
    using WebChat.Models;

    public class RoomController : BaseApiController
    {
        private readonly IGenericRepository<Room> roomRepository;

        private readonly IGenericRepository<Message> messagesRepository;

        public RoomController()
            : this(new WebChatData())
        {
        }

        public RoomController(IWebChatData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpGet]
        [ActionName("allrooms")]
        public IHttpActionResult GetAll()
        {
            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<RoomViewModel> rooms = this.Data.Rooms.GetAll()
                .Select(RoomViewModel.Create).OrderByDescending(r => r.Name).AsEnumerable();

            //var messages = this.Data.Messages.GetAll()
            //    .Where(m => m.Room.Id == room.Id)
            //    .Select(MessageViewModel.Create)
            //    .OrderByDescending(m => m.SentDate)
            //    .AsQueryable();

            return Ok(rooms);
        }

        [Authorize]
        [HttpPost]
        [ActionName("createroom")]
        public IHttpActionResult CreateRoom(CreateRoomBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Room room = new Room
            {
                Password = model.Password,
                Type = Convert.ParseRoomType(model.Type),
                Size = model.Size,
                Name = model.Name
            };

            roomRepository.Add(room);
            roomRepository.SaveChanges();

            return Ok(string.Format("Room with id: {0}, successfully created", room.Id));
        }

        [Authorize]
        [HttpDelete]
        [ActionName("deleteroom")]
        public IHttpActionResult DeleteRoom(DeleteRoomBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Room room = Data.Rooms.GetById(model.RoomId);

            if (room == null)
            {
                return Ok(string.Format("No room with id: {0}", model.RoomId));
            }

            roomRepository.Delete(room);
            roomRepository.SaveChanges();

            return Ok(string.Format("Room with id: {0}, successfully deleted", model.RoomId));
        }

        [Authorize]
        [HttpPut]
        [ActionName("updateroom")]
        public IHttpActionResult UpdateRoom(UpdateRoomBindingModel model)
        {
            string userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Room room = Data.Rooms.GetById(model.RoomId);

            if (room == null)
            {
                return Ok(string.Format("No room with id: {0}", model.RoomId));
            }

            if (model.Password != null)
            {
                room.Password = model.Password;
            }
            if (model.Size != 0)
            {
                room.Size = model.Size;
            }
            if (model.Type != null)
            {
                room.Type = Convert.ParseRoomType(model.Type);
            }

            roomRepository.SaveChanges();

            return Ok(string.Format("Room with id: {0}, successfully updated", model.RoomId));
        }
    }
}