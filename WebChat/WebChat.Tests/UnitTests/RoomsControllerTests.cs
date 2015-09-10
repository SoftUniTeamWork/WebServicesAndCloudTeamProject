using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebChat.Models;
using WebChat.Services.Controllers;
using WebChat.Services.Models.BindingModels;
using WebChat.Services.Models.ViewModels;
using WebChat.Tests.MockedObjects;

namespace WebChat.Tests.UnitTests
{
    [TestClass]
    public class RoomsControllerTests
    {
        private RoomsController controller;
        private JavaScriptSerializer serializer;
        private WebChatDataMock data;

        [TestInitialize]
        public void Initialize()
        {
            this.data = new WebChatDataMock();

            this.controller = new RoomsController(this.data, IdProviderMock.GetUserIdProvider().Object);
            this.serializer = new JavaScriptSerializer();
            this.Setup();
        }

        [TestMethod]
        public void TestGettingAllRoomsShouldReturnAllRooms()
        {
            var httpResponse = this.controller.GetAll().ExecuteAsync(new CancellationToken()).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;
            var rooms = this.serializer.Deserialize<IList<RoomViewModel>>(serverResponseJson);
            var responseRoomsJson = this.serializer.Serialize(rooms);

            var expectedResult = GetExpectedRoomsResult().OrderByDescending(r => r.Name);
            var expectedJson = this.serializer.Serialize(expectedResult);

            Assert.AreEqual(expectedJson, responseRoomsJson);
        }

        [TestMethod]
        public void GetUsersByRoomShouldReturnUsersInRoom()
        {
            var roomUser =
                this.data.Rooms.GetAll()
                    .FirstOrDefault(r => r.Id == 1)
                    .Users.First();

            var httpResponse = this.controller.GetUsersByRoomId(1).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            var users = this.serializer.Deserialize<IList<GetUsersByRoomModel>>(serverResponseJson);

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(users.First().Id, roomUser.Id);
        }

        [TestMethod]
        public void GettingUsersByRoomWithInvalidRoomIdShouldFail()
        {
            var httpResponse = this.controller.GetUsersByRoomId(5).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual("{\"Message\":\"Invalid room id\"}", serverResponseJson);
        }

        [TestMethod]
        public void GetRoomByIdShouldReturnRoom()
        {
            var expectedRoom = new RoomViewModel()
            {
                Name = "FirstRoom"
            };

            var httpResponse = this.controller
                .GetRoomById(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            var room = this.serializer.Deserialize<RoomViewModel>(serverResponseJson);

            Assert.AreEqual(expectedRoom.Name, room.Name);
        }

        [TestMethod]
        public void GetRoomByIdWhenRoomDoesNotExistShouldFail()
        {
            var httpResponse = this.controller
                    .GetRoomById(5)
                    .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual("{\"Message\":\"Room with such id doesn't exist\"}", serverResponseJson);
        }

        [TestMethod]
        public void AddingRoomShouldAddRoom()
        {
            var room = new CreateRoomBindingModel()
            {
                Name = "new room",
            };

            var httpResponse = this.controller.CreateRoom(room)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var resultRoomName =
                this.data.Rooms.GetAll()
                .Select(r => r.Name)
                .Last();

            Assert.AreEqual(room.Name, resultRoomName);
        }

        [TestMethod]
        public void DeleteRoomShouldDeleteRoom()
        {
            var room = this.data.Rooms.GetAll().First();
            var initialRoomsCount =
                this.data.Rooms.GetAll().Count();

            var httpResponse = this.controller.DeleteRoom(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            Assert.AreNotEqual(room.Id,
                this.data.Rooms.GetAll().First().Id);

            Assert.AreEqual(initialRoomsCount - 1,
                this.data.Rooms.GetAll().Count());
        }

        [TestMethod]
        public void DeleteRoomWhenRoomDoesNotExistShouldFail()
        {
            var httpResponse = this.controller.DeleteRoom(5)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [TestMethod]
        public void JoinRoomShouldJoinRoom()
        {
            var initialMembersCount =
                this.data.Rooms.GetAll()
                    .FirstOrDefault(r => r.Id == 1)
                    .Users.Count;

            var httpResponse = this.controller.JoinRoom(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var resultMembersCount =
                this.data.Rooms.GetAll()
                    .FirstOrDefault(r => r.Id == 1)
                    .Users.Count;

            Assert.AreEqual(initialMembersCount + 1, resultMembersCount);

            var joinedUser =
                this.data.Rooms.GetAll()
                    .FirstOrDefault(r => r.Id == 1)
                    .Users.Last();

            var expectedUser =
                this.data.Users.GetAll()
                    .First();

            Assert.AreEqual(expectedUser, joinedUser);

            var newLog = this.data.UserRoomSessions.GetAll()
                .Last();

            Assert.AreEqual(expectedUser, newLog.User);
        }

        [TestMethod]
        public void JoinRoomWhenRoomDoesNotExistShouldFail()
        {
            var httpResponse = this.controller.JoinRoom(5)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [TestMethod]
        public void QuitRoomShouldLeaveRoom()
        {
            var joinRoomHttpResponse =
                this.controller.JoinRoom(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, joinRoomHttpResponse.StatusCode);

            var initialMembersCount =
                this.data.Rooms.GetAll()
                    .FirstOrDefault(r => r.Id == 1)
                    .Users.Count;

            var leaveRoomHttpResponse =
                this.controller.QuitRoom(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, leaveRoomHttpResponse.StatusCode);

            var resultMembersCount =
                this.data.Rooms.GetAll()
                    .FirstOrDefault(r => r.Id == 1)
                    .Users.Count;

            Assert.AreEqual(initialMembersCount - 1,
                resultMembersCount);

            var newLog = this.data.UserRoomSessions.GetAll()
                .Last();

            Assert.IsNotNull(newLog.QuitDate);
        }

        [TestMethod]
        public void LeaveRoomWhenRoomDoesNotExistShouldFail()
        {
            var leaveRoomHttpResponse =
                this.controller.QuitRoom(5)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, leaveRoomHttpResponse.StatusCode);
        }

        private void Setup()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "rooms" } });

            this.controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            this.controller.Request = request;
            this.controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        private IList<RoomViewModel> GetExpectedRoomsResult()
        {
            return new List<RoomViewModel>()
            {
                new RoomViewModel()
                {
                    Id = 1,
                    Name = "FirstRoom",
                    Users = new List<UsersViewModel>()
                    {
                        new UsersViewModel()
                        {
                           Email = "mamamia@mamamia.com",
                            PhoneNumber = "088752235",
                            Username = "mamamia",
                            Id = WebChatDataMock.MockedUserId 
                        }
                    }
                },
                new RoomViewModel()
                {
                    Id = 2,
                    Name = "SecondRoom",
                    Users = new List<UsersViewModel>()
                    {
                        new UsersViewModel()
                        {
                           Email = "mamamia@mamamia.com",
                            PhoneNumber = "088752235",
                            Username = "mamamia",
                            Id = WebChatDataMock.MockedUserId 
                        }
                    }
                },
                new RoomViewModel()
                {
                    Id = 3,
                    Name = "ThirdRoom",
                    Users = new List<UsersViewModel>()
                    {
                        new UsersViewModel()
                        {
                            Email = "mamamia@mamamia.com",
                            PhoneNumber = "088752235",
                            Username = "mamamia",
                            Id = WebChatDataMock.MockedUserId 
                        }
                    }
                },
            };
        } 
    }
}
