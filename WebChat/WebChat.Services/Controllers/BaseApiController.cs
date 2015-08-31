using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebChat.DataLayer;
using WebChat.DataLayer.Repositories;

namespace WebChat.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        
        public BaseApiController()
            : this(new WebChatContext())
        {

        }

        public BaseApiController(WebChatContext data)
        {
            this.Data = data;
        }

        public WebChatContext Data { get; set; }
    }
}
