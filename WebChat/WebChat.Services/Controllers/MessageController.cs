using System.Linq;
using System.Web.Http;
using WebChat.DataLayer;
using WebChat.Services.Models;
using WebChat.Models;

namespace WebChat.Services.Controllers
{
    public class MessageController : BaseApiController
    {
        protected MessageController(IWebChatContext data)
            : base(data)
        {
        }

        [HttpPost]
        [ActionName("addmessage")]
        public IHttpActionResult AddMessage(AddMessageBindingModel model)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be null (no data in request)");
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
                PosterId = model.PosterId
            };

            Data.Messages.Add(message);
            Data.SaveChanges();

            return Ok(string.Format("Succesfully added post from user with id: {0}", poster.Id));
        }
    }
}