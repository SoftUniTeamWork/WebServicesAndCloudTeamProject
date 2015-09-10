using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WebChat.DataLayer.Contracts;
using WebChat.DataLayer.Data;
using WebChat.Models;
using WebChat.Services.Models.BindingModels;
using WebChat.Services.Models.ViewModels;
using WebChat.Services.Providers;
using WebChat.Services.UserSessionUtilities;
using Convert = WebChat.Services.Models.Utilities.Convert;

namespace WebChat.Services.Controllers
{
    public class RoomsController : BaseApiController
    {
        private readonly IIdProvider idProvider;
        public RoomsController()
            : base(new WebChatData())
        {
            
        }

        public RoomsController(IWebChatData data, IIdProvider idProvider)
            : base(data)
        {
            this.idProvider = idProvider;
        }

        [SessionAuthorize]
        [HttpGet]
        [ActionName("allrooms")]
        public IHttpActionResult GetAll()
        {
            var userId = this.idProvider.GetId();
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
            var userId = this.idProvider.GetId();

            if (userId == null)
            {
                return this.Unauthorized();
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var roomNameExists = this.Data.Rooms.GetAll().Any(r => r.Name == model.Name);
            if (roomNameExists)
            {
                return this.BadRequest("Room name already exists");
            }

            var room = new Room
            {
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
            var userId = this.idProvider.GetId();
            var room = Data.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId);
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
                return BadRequest(string.Format("No room with id: {0}", roomId));
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
            var userId = this.idProvider.GetId();
            var user = this.Data.Users.GetAll().FirstOrDefault(u => u.Id == userId);
            var room = this.Data.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            if (room == null)
            {
                return this.BadRequest("There is no room with such id");
            }
            
            var session = new UserRoomSession()
            {
                QuitDate = DateTime.Now,
                User = user,
                Room = room
            };
            room.Users.Add(user);
            this.Data.UserRoomSessions.Add(session);
            this.Data.SaveChanges();

            return this.Ok("User has successfully joined the room!");
        }

        [SessionAuthorize]
        [HttpPost]
        [Route("api/rooms/{roomId}/quit")]
        public IHttpActionResult QuitRoom(int roomId)
        {
            var room = this.Data.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId);
            var userId = this.idProvider.GetId();
            var user = this.Data.Users.GetAll().FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return this.Unauthorized();
            }

            if (room == null)
            {
                return this.BadRequest("Room with such id doesn't exist!");
            }

            room.Users.Remove(user);
            var userRoomSession = this.Data.UserRoomSessions.GetAll()
                .FirstOrDefault(urs => urs.Room.Id == roomId && urs.User.Id == userId);
            userRoomSession.QuitDate = DateTime.Now;
            
            this.Data.SaveChanges();

            return this.Ok("User quited the room successfully!");
        }

        [HttpGet]
        [Route("api/rooms/{roomId}/users")]
        public IHttpActionResult GetUsersByRoomId(int roomId)
        {
            var room = this.Data.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                return this.BadRequest("Invalid room id");
            }

            var users = room.Users.Select(m => new GetUsersByRoomModel()
            {
                Id = m.Id,
                Username = m.UserName
            }).AsQueryable();

            return this.Ok(users);
        }

        [HttpGet]
        public IHttpActionResult GetRoomById(int id)
        {
            var room = this.Data.Rooms.GetAll()
                .Select(RoomViewModel.Create)
                .FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return this.BadRequest("Room with such id doesn't exist");
            }

            return this.Ok(room);
        }

    }
}