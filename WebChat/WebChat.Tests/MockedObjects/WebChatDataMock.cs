using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebChat.DataLayer.Contracts;
using WebChat.Models;

namespace WebChat.Tests.MockedObjects
{
    public class WebChatDataMock : IWebChatData
    {
        public const string MockedUserId = "someId";

        private IGenericRepository<ApplicationUser> users;
        private IGenericRepository<Message> messages;
        private IGenericRepository<Room> rooms;
        private IGenericRepository<Tag> tags;
        private IGenericRepository<Notification> notifications;
        private IGenericRepository<UserSession> sessions;
        private IGenericRepository<UserRoomSession> history;

        public WebChatDataMock()
        {
            this.users = new GenericRepositoryMock<ApplicationUser>();
            this.rooms = new GenericRepositoryMock<Room>();
            this.messages = new GenericRepositoryMock<Message>();
            this.history = new GenericRepositoryMock<UserRoomSession>();
            this.sessions = new GenericRepositoryMock<UserSession>();
            this.tags = new GenericRepositoryMock<Tag>();
            this.notifications = new GenericRepositoryMock<Notification>();

            SeedMockedUsers(this);
            SeedMockedMessages(this);
            SeedMockedRooms(this);
        }

        public bool IsSaveCalled { get; set; }

        public IGenericRepository<ApplicationUser> Users
        {
            get { return this.users; }
        }

        public IGenericRepository<Room> Rooms
        {
            get { return this.rooms; }
        }


        public IGenericRepository<Message> Messages
        {
            get { return this.messages; }
        }

        public IGenericRepository<UserSession> UserSessions
        {
            get { return this.sessions; }
        }

        public IGenericRepository<UserRoomSession> UserRoomSessions
        {
            get { return this.history; }
        }

        public IGenericRepository<Notification> Notifications
        {
            get { return this.notifications; }
        }

        public IGenericRepository<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> UserRoles
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Tag> Tags
        {
            get { return this.Tags; }
        }

        public int SaveChanges()
        {
            this.IsSaveCalled = true;
            return 1;
        }

        public static IList<Message> GetMockedMessagesList()
        {
            return new List<Message>()
            {
                new Message()
                {
                    Text = "FirstMessage",
                    SentDate = new DateTime(2010, 5, 5),
                    Id = 1,
                    PosterId = MockedUserId,
                    Poster = GetMockedUser(),
                    RoomId = 1
                },
                new Message()
                {
                    Text = "SecondMessage",
                    SentDate = new DateTime(2014, 4, 9),
                    Id = 2,
                    PosterId = MockedUserId,
                    Poster = GetMockedUser(),
                    RoomId = 1
                },
                new Message()
                {
                    Text = "ThirdMessage",
                    SentDate = new DateTime(2015, 3, 2),
                    Id = 3,
                    PosterId = MockedUserId,
                    Poster = GetMockedUser(),
                    RoomId = 1
                },
                new Message()
                {
                    Text = "FourthMessage",
                    SentDate = new DateTime(2015, 3, 2),
                    Id = 3,
                    PosterId = MockedUserId,
                    Poster = GetMockedUser(),
                    RoomId = 1
                }
            };
        }

        public static IList<Room> GetMockedRoomsList()
        {
            return new List<Room>()
            {
                new Room()
                {
                    Id = 1,
                    Name = "FirstRoom",
                    Users = new List<ApplicationUser>(){GetMockedUser()},
                    Messages = GetMockedMessagesList()
                },
                new Room()
                {
                    Id = 2,
                    Name = "SecondRoom",
                    Users = new List<ApplicationUser>(){GetMockedUser()},
                    Messages = GetMockedMessagesList()
                },
                new Room()
                {
                    Id = 3,
                    Name = "ThirdRoom",
                    Users = new List<ApplicationUser>(){GetMockedUser()},
                    Messages = GetMockedMessagesList()
                },
            };
        }

        public static ApplicationUser GetMockedUser()
        {
            return new ApplicationUser()
            {
                Email = "mamamia@mamamia.com",
                PhoneNumber = "088752235",
                UserName = "mamamia",
                Id = MockedUserId
            };
        }

        private static void SeedMockedUsers(WebChatDataMock data)
        {
            data.Users.Add(GetMockedUser());
        }

        private static void SeedMockedRooms(WebChatDataMock data)
        {
            data.Rooms.Add(new Room()
            {
                Id = 1,
                Name = "FirstRoom",
                Users = new List<ApplicationUser>() { GetMockedUser() },
                Messages = data.Messages.GetAll().ToList()
            });

            data.Rooms.Add(new Room()
            {
                Id = 2,
                Name = "SecondRoom",
                Users = new List<ApplicationUser>() { GetMockedUser() },
                Messages = data.Messages.GetAll().ToList()
            });

            data.Rooms.Add(new Room()
            {
                Id = 3,
                Name = "ThirdRoom",
                Users = new List<ApplicationUser>() { GetMockedUser() },
                Messages = data.Messages.GetAll().ToList()
            });
        }

        private static void SeedMockedMessages(WebChatDataMock data)
        {
            data.Messages.Add(new Message()
            {
                Text = "FirstMessage",
                SentDate = new DateTime(2000, 1, 1),
                Id = 1,
                PosterId = MockedUserId,
                Poster = GetMockedUser(),
                RoomId = 1
            });

            data.Messages.Add(new Message()
            {
                Text = "SecondMessage",
                SentDate = new DateTime(1999, 1, 1),
                Id = 2,
                PosterId = MockedUserId,
                Poster = GetMockedUser(),
                RoomId = 1
            });

            data.Messages.Add(new Message()
            {
                Text = "ThirdMessage",
                SentDate = new DateTime(1999, 5, 1),
                Id = 3,
                PosterId = MockedUserId,
                Poster = GetMockedUser(),
                RoomId = 1
            });
        }


        



        
    }
}
