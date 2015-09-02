namespace WebChat.Services.Controllers
{
    using System.Web.Http;
    using DataLayer.Contracts;
    public class BaseApiController : ApiController
    {
        public BaseApiController(IWebChatData data)
        {
            this.Data = data;
        }

        protected IWebChatData Data { get; set; }
    }
}