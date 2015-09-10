using WebChat.Services.Providers;

namespace WebChat.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Script.Serialization;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Testing;
    using DataLayer;
    using DataLayer.Contracts;
    using DataLayer.Data;
    using WebChat.Models;
    using Models.BindingModels.User;
    using UserSessionUtilities;

    [SessionAuthorize]
    [RoutePrefix("api/user")]
    public class AccountController : BaseApiController
    {
        private ApplicationUserManager userManager;
        private readonly IIdProvider idProvider;

        public AccountController(IWebChatData data, IIdProvider idProvider)
            : base(data)
        {
            this.idProvider = idProvider;
        }

        public AccountController()
            : base(new WebChatData())
        {
            this.userManager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(new WebChatContext()));
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
        }

        private IAuthenticationManager Authentication
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        // POST api/User/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> RegisterUser(RegisterUserBindingModel model)
        {
            if (this.idProvider.GetId() != null)
            {
                return this.BadRequest("User is already logged in.");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (this.Data.Users.GetAll().Any(u => u.Email == model.Email))
            {
                return this.BadRequest("That email is already used by another user");
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.Phone
            };

            var identityResult = await this.UserManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                return this.GetErrorResult(identityResult);
            }

            // Auto login after registration (successful user registration should return access_token)
            var loginResult = await this.LoginUser(new LoginUserBindingModel()
            {
                Username = model.Username,
                Password = model.Password
            });
            return loginResult;
        }

        // POST api/User/Login
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> LoginUser(LoginUserBindingModel model)
        {
            if (this.idProvider.GetId() != null)
            {
                return this.BadRequest("Already logged in!");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            // Invoke the "token" OWIN service to perform the login (POST /api/token)
            // Use Microsoft.Owin.Testing.TestServer to perform in-memory HTTP POST request
            var testServer = TestServer.Create<Startup>();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };
            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var tokenServiceResponse = await testServer.HttpClient.PostAsync(
                Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

            if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
            {
                // Sucessful login --> create user session in the database
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var jsSerializer = new JavaScriptSerializer();
                var responseData =
                    jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
                var authToken = responseData["access_token"];
                var username = responseData["username"];
                var userSessionManager = new UserSessionManager(this.Request.GetOwinContext());
                userSessionManager.CreateUserSession(username, authToken);

                // Cleanup: delete expired sessions from the database
                userSessionManager.DeleteExpiredSessions();
            }

            return this.ResponseMessage(tokenServiceResponse);
        }

        // POST api/User/Logout
        [HttpPost]
        [SessionAuthorize]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            // This does not actually perform logout! The OWIN OAuth implementation
            // does not support "revoke OAuth token" (logout) by design.
            this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);

            // Delete the user's session from the database (revoke its bearer token)
            var userSessionManager = new UserSessionManager(this.Request.GetOwinContext());
            userSessionManager.InvalidateUserSession();

            return this.Ok(
                new
                {
                    message = "Logout successful."
                }
            );
        }

        // POST api/User/Ads
        [HttpPost]
        [Route("Message")]
        public IHttpActionResult CreateNewMessage(UserCreateMessageBindingModel model)
        {
            // Validate the input parameters
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            // Validate that the current user exists in the database
            var currentUserId = this.idProvider.GetId();
            var currentUser = this.Data.Users.GetAll().FirstOrDefault(x => x.Id == currentUserId);

            if (currentUser == null)
            {
                return this.BadRequest("Invalid user token! Please login again!");
            }

            var currentRoom = currentUser.Room;

            if (currentRoom == null)
            {
                return this.BadRequest("User currently isn't in any room!");
            }


            var message = new Message()
            {
                Text = model.Text,
                PosterId = currentUserId,
                SentDate = DateTime.Now,
                Room = currentRoom
            };

            this.Data.Messages.Add(message);

            this.Data.SaveChanges();

            return this.Ok(
                new
                {
                    message = string.Format("Message from user with id {0} created successfully.", currentUserId),
                    messageId = message.Id
                }
            );
        }

        // DELETE api/User/Messages/{id}
        [HttpDelete]
        [Route("Messages/{id:int}")]
        public IHttpActionResult DeleteMessage(int id)
        {
            var message = this.Data.Messages.GetAll().FirstOrDefault(d => d.Id == id);
            if (message == null)
            {
                return this.BadRequest("Message #" + id + " not found!");
            }

            // Validate the current user ownership over the message
            var currentUserId = this.idProvider.GetId();
            if (message.PosterId != currentUserId)
            {
                return this.Unauthorized();
            }

            //TODO: RoomAdmin can delete messages
            this.Data.Messages.Delete(message);

            this.Data.Messages.SaveChanges();

            return this.Ok(
               new
               {
                   message = "Message #" + id + " deleted successfully."
               }
           );
        }

        // PUT api/User/ChangePassword
        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangeUserPassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IdentityResult result = await this.UserManager.ChangePasswordAsync(
                this.idProvider.GetId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok(
                new
                {
                    message = "Password changed successfully.",
                }
            );
        }

        // GET api/Users/Profile
        [HttpGet]
        [Route("Profile")]
        public IHttpActionResult GetUserProfile()
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            // Validate the current user exists in the database
            var currentUserId = this.idProvider.GetId();
            var currentUser = this.Data.Users.GetAll().FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser == null)
            {
                return this.BadRequest("Invalid user token! Please login again!");
            }

            var userToReturn = new
            {
                currentUser.UserName,
                currentUser.Email,
                currentUser.PhoneNumber
            };

            return this.Ok(userToReturn);
        }

        // PUT api/Users/Profile
        [HttpPut]
        [Route("Profile")]
        public IHttpActionResult EditUserProfile(EditUserProfileBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            // Validate the current user exists in the database
            var currentUserId = this.idProvider.GetId();
            var currentUser = this.Data.Users.GetAll().FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser == null)
            {
                return this.BadRequest("Invalid user token! Please login again!");
            }

            var emailExists = this.Data.Users.GetAll().Any(x => x.Email == model.Email);
            if (emailExists)
            {
                return this.BadRequest("Invalid email. This email is already used!");
            }

            currentUser.UserName = model.Username;
            currentUser.Email = model.Email;
            currentUser.PhoneNumber = model.PhoneNumber;

            this.Data.SaveChanges();

            return this.Ok(
                new
                {
                    message = "User profile edited successfully.",
                });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.UserManager.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}