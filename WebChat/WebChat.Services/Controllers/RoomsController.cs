using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WebChat.DataLayer.Contracts;
using WebChat.DataLayer.Data;
using WebChat.Models;
using WebChat.Services.Models.BindingModels;
using WebChat.Services.Models.ViewModels;
using WebChat.Services.UserSessionUtilities;
using Convert = WebChat.Services.Models.Utilities.Convert;

namespace WebChat.Services.Controllers
{
    public class RoomsController : BaseApiController
    {
        public RoomsController()
            : this(new WebChatData())
        {
        }

        public RoomsController(IWebChatData data)
            : base(data)
        {
        }

        [SessionAuthorize]
        [HttpGet]
        [ActionName("allrooms")]
        public IHttpActionResult GetAll()
        {
            var userId = this.User.Identity.GetUserId();
            var rooms = this.Data.Rooms.GetAll()
                .Select(RoomViewModel.Create).OrderByDescending(r => r.Name);
            if (userId == null)
            {
                return this.Unauthorized();
             }
             
            return this.Ok(rooms);
        }


        [SessionAuthorize]
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

            var room = new Room
            {
                Password = model.Password,
                Type = Convert.ParseRoomType(model.Type),
                Size = model.Size,
                Name = model.Name
            };

            this.Data.Rooms.Add(room);
            this.Data.SaveChanges();

            return Ok(string.Format("Room with id: {0}, successfully created", room.Id));
        }

        [SessionAuthorize]
        [HttpDelete]
        [ActionName("deleteroom")]
        public IHttpActionResult DeleteRoom(int roomId)
        {
            var userId = this.User.Identity.GetUserId();
            var room = Data.Rooms.GetById(roomId);
            if (userId == null)
            {
                return this.Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (room == null)
            {
                return Ok(string.Format("No room with id: {0}", roomId));
            }

            this.Data.Rooms.Delete(room);
            this.Data.Rooms.SaveChanges();

            return Ok(string.Format("Room with id: {0}, successfully deleted", roomId));
        }

        [SessionAuthorize]
        [HttpPost]
        [Route("api/rooms/{roomId}/join")]
        public IHttpActionResult JoinRoom(int roomId)
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);
            var room = this.Data.Rooms.GetById(roomId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            if (room == null)
            {
                this.BadRequest("There is no room with such id");
            }
            
            var session = new UserRoomSession()
            {
                JoinDate = DateTime.Now,
                User = user,
                Room = room
            };

            this.Data.UserRoomSessions.Add(session);
            this.Data.SaveChanges();

            return this.Ok("User has successfully joined the room!");
        }
        /*
        [SessionAuthorize]
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

            this.Data.Rooms.SaveChanges();

            return Ok(string.Format("Room with id: {0}, successfully updated", model.RoomId));
        }
        */
        [SessionAuthorize]
        [HttpPost]
        [Route("api/rooms/{roomId}/quit")]
        public IHttpActionResult QuitRoom(int roomId)
        {
            var room = this.Data.Rooms.GetById(roomId);
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);
            if (user == null)
            {
                return this.Unauthorized();
            }

            if (room == null)
            {
                return this.BadRequest("Room with such id doesn't exist!");
            }

            room.Users.Remove(user);
            this.Data.SaveChanges();

            return this.Ok("User quited the room successfully!");
        }
    }
}