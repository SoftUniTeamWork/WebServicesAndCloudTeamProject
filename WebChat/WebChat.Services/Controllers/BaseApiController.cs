using System.Web.Http;
using WebChat.DataLayer;

namespace WebChat.Services.Controllers
{
    public class BaseApiController : ApiController
    {

        protected BaseApiController() 
            : this (new WebChatContext())
        {
        }

        public BaseApiController(WebChatContext data)
        {
            this.Data = data;
        }
        protected WebChatContext Data { get; set; }
    }
}