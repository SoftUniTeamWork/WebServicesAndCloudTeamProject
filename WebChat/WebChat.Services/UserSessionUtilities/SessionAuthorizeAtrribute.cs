
namespace WebChat.Services.UserSessionUtilities
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    using DataLayer.Contracts;
    using DataLayer.Data;

    public class SessionAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
     {
        protected IWebChatData Data { get; private set; }

        public SessionAuthorizeAttribute(IWebChatData data)
        {
            this.Data = data;
        }

        public SessionAuthorizeAttribute()
            : this(new WebChatData())
        {
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
            {
                return;
            }   

            var userSessionManager = new UserSessionManager();
            if (userSessionManager.ReValidateSession())
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized, "Session token expried or not valid.");
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}