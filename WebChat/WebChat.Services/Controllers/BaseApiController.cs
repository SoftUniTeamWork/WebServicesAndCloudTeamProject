namespace WebChat.Services.Controllers
{
    using System.Web.Http;
    using DataLayer;
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