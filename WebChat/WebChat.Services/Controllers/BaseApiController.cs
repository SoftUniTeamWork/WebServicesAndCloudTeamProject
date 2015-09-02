using Microsoft.AspNet.Identity;

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

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}