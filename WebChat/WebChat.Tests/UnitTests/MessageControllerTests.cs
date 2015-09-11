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
    public class MessageControllerTests
    {
        private MessageController controller;
        private JavaScriptSerializer serializer;
        private WebChatDataMock dataMock;

        [TestInitialize]
        public void Initialize()
        {
            this.dataMock = new WebChatDataMock();

            this.controller = new MessageController(this.dataMock, IdProviderMock.GetUserIdProvider().Object);
            this.serializer = new JavaScriptSerializer();
            this.Setup();
        }

        [TestMethod]
        public void TestGettingAllMessagesShouldReturnAllMessages()
        {
            var httpResponse = this.controller.GetAllMessages(1).ExecuteAsync(new CancellationToken()).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var messages = this.serializer.Deserialize<IList<MessageOutputModel>>(serverResponseJson).OrderBy(m => m.Id)
                .Select(m => m.Id)
                .ToList();

            var expectedResult = GetExpectedMessagesResult()
                .Select(m => m.Id)
                .ToList();

            CollectionAssert.AreEqual(expectedResult, messages);
        }

        [TestMethod]
        public void GettingAllMessagesWithInvalidRoomIdShouldFail()
        {
            var httpResponse = this.controller.GetAllMessages(-1).ExecuteAsync(CancellationToken.None).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            Assert.AreEqual("{\"Message\":\"Invalid room id\"}", serverResponseJson);
        }

        [TestMethod]
        public void GettingAllMessagesForEmptyRoomShouldReturnZeroMessages()
        {
            this.dataMock.Rooms.Add(new Room()
            {
                Id = 5,
                Name = "Room 5",
                Messages = new Message[0]
            });

            var httpResponse = this.controller.GetAllMessages(5).ExecuteAsync(new CancellationToken()).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var messagesCount = this.serializer.Deserialize<IList<MessageOutputModel>>(serverResponseJson)
                .Count;

            Assert.AreEqual(0, messagesCount);
        }

        [TestMethod]
        public void AddingMessageShouldAddMessage()
        {
            var message = new CreateMessageBindingModels()
            {
                Text = "New message"
            };

            var expectedMessage = new MessageOutputModel()
            {
                Text = "New message",
                SentDate = DateTime.Now,
                Id = 4,
                RoomId = 1,
                PosterName = WebChatDataMock.MockedUserId
            };

            var httpResponse = this.controller.CreateMessage(1, message).ExecuteAsync(new CancellationToken()).Result;

            var serverResponse = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(serverResponse, "\"Message successfully created\"");

            Assert.AreEqual(expectedMessage.Text, this.dataMock.Messages.GetAll().Last().Text);
        }

        [TestMethod]
        public void AddingMessageWithInvalidRoomIdShouldFail()
        {
            var message = new CreateMessageBindingModels()
            {
                Text = "New message"
            };

            var httpResponse = this.controller.CreateMessage(5, message).ExecuteAsync(new CancellationToken()).Result;

            var serverResponse = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual("{\"Message\":\"Room doesn't exist\"}", serverResponse);

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }


        /*
        [TestMethod]
        public void GettingLatestMessagesShouldReturnLatestMessages()
        {
            var message = new MessageInputModel()
            {
                Content = "New message"
            };
            var addMessageResponse = this.controller.AddMessage(1, message).ExecuteAsync(new CancellationToken());
            var httpResponseTask = this.controller
                    .GetLatestMessages(1)
                    .ExecuteAsync(new CancellationToken()).Result;
            var httpResponse = httpResponseTask;
            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            var serverResponse = httpResponse.Content
                .ReadAsStringAsync().Result;
            var messageId = this.serializer.Deserialize<IList<MessageOutputModel>>(serverResponse)
                .Select(m => m.Id)
                .FirstOrDefault();
            Assert.AreEqual(messageId, this.unitOfWorkMock.Messages.All().Last().Id);
        }
        */

        /*
        [TestMethod]
        public void AddingMessageWithInvalidInputModelShouldFail()
        {
            var message = new MessageInputModel();
            var httpResponse = this.controller.AddMessage(1, message).ExecuteAsync(new CancellationToken()).Result;
            var serverResponse = httpResponse.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(serverResponse, "\"Invalid input model\"");
            Assert.AreEqual(httpResponse.StatusCode, HttpStatusCode.BadRequest);
        }
        */

        private void Setup()
        {

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "messages" } });

            this.controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            this.controller.Request = request;
            this.controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        private static IList<MessageOutputModel> GetExpectedMessagesResult()
        {
            return new List<MessageOutputModel>()
            {
                new MessageOutputModel()
                {
                    Id = 1,
                    Text = "1",
                    SentDate = new DateTime(1989, 7, 22),
                    PosterId = WebChatDataMock.MockedUserId,
                    RoomId = 1
                },
                new MessageOutputModel()
                {
                    Id = 2,
                    Text = "2",
                    SentDate = new DateTime(1999, 3, 10),
                    PosterId = WebChatDataMock.MockedUserId,
                    RoomId = 1
                },
                new MessageOutputModel()
                {
                    Id = 3,
                    Text = "3",
                    SentDate = new DateTime(2099, 9, 9),
                    PosterId = WebChatDataMock.MockedUserId,
                    RoomId = 1
                }
            };
        }
    }
}
