using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WebChat.DataLayer;
using WebChat.Services.Models;
using WebChat.Models;

namespace WebChat.Services.Controllers
{

    public class MessageController : BaseApiController
    {
        [Authorize]
        [HttpPost]
        [Route("api/rooms/{roomId}/messages")]
        public IHttpActionResult AddMessage(int roomId, AddMessageBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();
            var room = this.Data.Rooms.FirstOrDefault(r => r.Id == roomId);
            var sendingUser = this.Data.Users
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

            var poster = Data.Users.FirstOrDefault(u => u.Id == model.PosterId);

            if (poster == null)
            {
                return this.BadRequest(string.Format("User with id:{0} does not exist", poster.Id));
            }

            var message = new Message
            {
                Text = model.Text,
                PosterId = model.PosterId,
                Sent = DateTime.Now,
                Room = room,
                RoomId = roomId
            };

            Data.Messages.Add(message);
            Data.SaveChanges();

            return Ok(string.Format("Message from user with id = {0} successfully sent!", poster.Id));
        }
    }
}