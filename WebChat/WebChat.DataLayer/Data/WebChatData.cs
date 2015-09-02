namespace WebChat.DataLayer.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Contracts;
    using Models;
    public class WebChatData : IWebChatData
    {
        private readonly DbContext context;

        private readonly IDictionary<Type, object> repositories;

        public WebChatData()
            : this(new WebChatContext())
        {
        }

        public WebChatData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }
        public IGenericRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetRepository<ApplicationUser>();
            }
        }

        public IGenericRepository<IdentityRole> UserRoles
        {
            get
            {
                return this.GetRepository<IdentityRole>();
            }
        }

        public IGenericRepository<Room> Rooms
        {
            get
            {
                return this.GetRepository<Room>();
            }
        }

        public IGenericRepository<Notification> Notifications
        {
            get
            {
                return this.GetRepository<Notification>();
            }
        }

        public IGenericRepository<Message> Messages
        {
            get
            {
                return this.GetRepository<Message>();
            }
        }

        public IGenericRepository<UserSession> UserSessions
        {
            get
            {
                return this.GetRepository<UserSession>();
            }
        }

        public IGenericRepository<Tag> Tags
        {
            get
            {
                return this.GetRepository<Tag>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(IGenericRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IGenericRepository<T>)this.repositories[typeof(T)];
        }
    }
}
