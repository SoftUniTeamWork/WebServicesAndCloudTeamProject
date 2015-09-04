using Microsoft.Owin;

namespace WebChat.Services.UserSessionUtilities
{
    using System;
    using System.Net.Http;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using WebChat.Models;
    using DataLayer.Contracts;
    using DataLayer.Data;

    public class UserSessionManager
    {
        private static readonly TimeSpan DefaultSessionTimeout = new TimeSpan(365,0,0,0);
        protected IWebChatData Data { get; private set; }
        protected IOwinContext Context { get; set; }

        public UserSessionManager(IWebChatData data, IOwinContext context)
        {
            this.Data = data;
            this.Context = context;
        }

        public UserSessionManager(IOwinContext context) 
            : this(new WebChatData(), context)
        {
        }

        /// <returns>The current bearer authorization token from the HTTP headers</returns>
        private string GetCurrentBearerAuthrorizationToken()
        {
            string authToken = null;
            if (this.Context.Request.Headers["Authorization"] != null)
            {
                authToken = this.Context.Request.Headers["Authorization"];
            }
            return authToken;
        }

        private string GetCurrentUserId()
        {
            if (Context.Authentication.User.Identity  == null)
            {
                return null;
            }

            return this.Context.Authentication.User.Identity.GetUserId();
        }

        /// <summary>
        /// Extends the validity period of the current user's session in the database.
        /// This will configure the user's bearer authorization token to expire after
        /// certain period of time (e.g. 30 minutes, see UserSessionTimeout in Web.config)
        /// </summary>
        public void CreateUserSession(string username, string authToken)
        {
            var userId = this.Data.Users.GetAll().First(u => u.UserName == username).Id;
            var userSession = new UserSession()
            {
                OwnerUserId = userId,
                AuthToken = authToken
            };
            this.Data.UserSessions.Add(userSession);

            // Extend the lifetime of the current user's session: current moment + fixed timeout
            userSession.ExpirationDateTime = DateTime.Now + DefaultSessionTimeout;
            this.Data.SaveChanges();
        }

        /// <summary>
        /// Makes the current user session invalid (deletes the session token from the user sessions).
        /// The goal is to revoke any further access with the same authorization bearer token.
        /// Typically this method is called at "logout".
        /// </summary>
        public void InvalidateUserSession()
        {
            string authToken = GetCurrentBearerAuthrorizationToken();
            var currentUserId = GetCurrentUserId();
            var userSession = this.Data.UserSessions.GetAll().FirstOrDefault(session =>
                session.AuthToken == authToken && session.OwnerUserId == currentUserId);
            if (userSession != null)
            {
                this.Data.UserSessions.Delete(userSession);
                this.Data.SaveChanges();
            }
        }

        /// <summary>
        /// Re-validates the user session. Usually called at each authorization request.
        /// If the session is not expired, extends it lifetime and returns true.
        /// If the session is expired or does not exist, return false.
        /// </summary>
        /// <returns>true if the session is valid</returns>
        public bool ReValidateSession()
        {
            string authToken = this.GetCurrentBearerAuthrorizationToken();
            if (authToken != null)
            {
                authToken = authToken.Substring(7);
            }

            var currentUserId = this.GetCurrentUserId();
            var userSession = this.Data.UserSessions.GetAll()
                .FirstOrDefault(session => session.AuthToken == authToken && session.OwnerUserId == currentUserId);

            if (userSession == null)
            {
                // User does not have a session with this token --> invalid session
                return false;
            }

            if (userSession.ExpirationDateTime < DateTime.Now)
            {
                // User's session is expired --> invalid session
                return false;
            }

            // Extend the lifetime of the current user's session: current moment + fixed timeout
            userSession.ExpirationDateTime = DateTime.Now + DefaultSessionTimeout;
            this.Data.SaveChanges();

            return true;
        }

        public void DeleteExpiredSessions()
        {
            var userSessions = this.Data.UserSessions.GetAll().Where(
                session => session.ExpirationDateTime < DateTime.Now);
            foreach (var session in userSessions)
            {
                this.Data.UserSessions.Delete(session);
            }
        }
    }
}