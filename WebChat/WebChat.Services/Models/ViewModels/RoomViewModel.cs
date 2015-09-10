using System.Collections.Generic;
using System.Linq;

namespace WebChat.Services.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Utilities;
    using WebChat.Models;

    public class RoomViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<UsersViewModel> Users { get; set; }

        public int UsersCount { get { return this.Users.Count(); } }

        public static Expression<Func<Room, RoomViewModel>> Create
        {
            get
            {
                return m => new RoomViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Users = m.Users.Select(u => new UsersViewModel()
                    {
                        Id = u.Id,
                        Username = u.UserName,
                        Email = u.Email,
                        PhoneNumber =  u.PhoneNumber
                    })
                    
                };
            }
        }
    }
}