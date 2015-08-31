using System.Web.Http;
using WebChat.DataLayer;

namespace WebChat.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        private IWebChatContext _data;

        protected BaseApiController(IWebChatContext data)
        {
            data = _data;
        }

        protected IWebChatContext Data
        {
            get { return _data; }
        }
    }
}