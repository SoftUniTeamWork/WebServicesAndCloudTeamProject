namespace WebChat.DataLayer.Data
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Contracts;
    using Repositories;
    using Models;
    public class WebChatData : IWebChatData
    {
        private readonly IWebChatContext context;

        private readonly IDictionary<Type, object> dataRepositories;

        public WebChatData()
            : this(new WebChatContext())
        {
        }

        public WebChatData(IWebChatContext context)
        {
            this.context = context;
            this.dataRepositories = new Dictionary<Type, object>();
        }
        public IGenericRepository<ApplicationUser> Users
        {
            get
            {
                return this.SetRepositoryType<ApplicationUser>();
            }
        }

        public IGenericRepository<IdentityRole> UserRoles
        {
            get
            {
                return this.SetRepositoryType<IdentityRole>();
            }
        }

        public IGenericRepository<Room> Rooms
        {
            get
            {
                return this.SetRepositoryType<Room>();
            }
        }

        public IGenericRepository<Notification> Notifications
        {
            get
            {
                return this.SetRepositoryType<Notification>();
            }
        }

        public IGenericRepository<Message> Messages
        {
            get
            {
                return this.SetRepositoryType<Message>();
            }
        }

        public IGenericRepository<UserSession> UserSessions
        {
            get
            {
                return this.SetRepositoryType<UserSession>();
            }
        }

        public IGenericRepository<Tag> Tags
        {
            get
            {
                return this.SetRepositoryType<Tag>();
            }
        }

        public IGenericRepository<UserRoomSession> UserRoomSessions
        {
            get { return this.SetRepositoryType<UserRoomSession>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IGenericRepository<T> SetRepositoryType<T>() where T : class
        {
            var typeOfModel = typeof(T);

            if (!this.dataRepositories.ContainsKey(typeOfModel))
            {
                var type = typeof(GenericRepositorty<T>);
                this.dataRepositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));
            }

            return this.dataRepositories[typeOfModel] as IGenericRepository<T>;
        }


        
    }
}
