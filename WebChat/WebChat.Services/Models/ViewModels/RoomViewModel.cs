namespace WebChat.Services.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Utilities;
    using WebChat.Models;

    public class RoomViewModel
    {
        public int Id { get; set; }

        public string Password { get; set; }

        public RoomType Type { get; set; }

        public int Size { get; set; }

        public string Name { get; set; }

        public static Expression<Func<Room, RoomViewModel>> Create
        {
            get
            {
                return m => new RoomViewModel()
                {
                    Id = m.Id,
                    Password = m.Password,
                    Type = m.Type,
                    Size = m.Size,
                    Name = m.Name
                };
            }
        }
    }
}