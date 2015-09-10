using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebChat.DataLayer.Data;
using WebChat.Services.Controllers;
using WebChat.Services.Models.BindingModels.User;
using WebChat.Services.Models.ViewModels;
using WebChat.Tests.MockedObjects;

namespace WebChat.Tests
{
    [TestClass]
    public class AccountControllerTests
    {

        private AccountController controller;
        private JavaScriptSerializer serializer;
        private WebChatDataMock dataMock;

        [TestInitialize]
        public void Initialize()
        {
            this.dataMock = new WebChatDataMock();
            this.serializer = new JavaScriptSerializer();
            this.controller = new AccountController(this.dataMock,IdProviderMock.GetUserIdProvider().Object);
            this.serializer = new JavaScriptSerializer();
            this.Setup();
        }

        [TestMethod]
        public void GetUserProfileShouldReturnUserProfileInfo()
        {
            var httpResponse = this.controller
                .GetUserProfile()
                .ExecuteAsync(new CancellationToken()).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var serverResponse = httpResponse.Content
                .ReadAsStringAsync().Result;

            var profile =
                this.serializer
                    .Deserialize<UsersViewModel>(serverResponse);

            var expectedProfile = this.dataMock
                .Users.GetAll()
                .FirstOrDefault();

            Assert.AreEqual(expectedProfile.Id, profile.Id);

        }

        [TestMethod]
        public void EditUserProfileShouldChangeProfileInfo()
        {
            var editUserInfo = new EditUserProfileBindingModel()
            {
                Username = "hahahaha",
                Email = "hahahaha@bg.com",
                PhoneNumber = "0887522235"
            };
            var response =
                this.controller.EditUserProfile(editUserInfo)
                    .ExecuteAsync(new CancellationToken()).Result;

            Assert.AreEqual(HttpStatusCode.OK,
                response.StatusCode);

            var editedName = this.dataMock
                .Users.GetAll()
                .Select(u => u.UserName)
                .FirstOrDefault();

            Assert.AreEqual<string>("hahahaha", editedName);
        }

        private void Setup()
        {

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary {{"controller", "messages"}});

            this.controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            this.controller.Request = request;
            this.controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }
    }
}

