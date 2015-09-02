namespace WebChat.Services.Models.Utilities
{
    using System;
    using WebChat.Models;

    public static class Convert
    {
        public static RoomType ParseRoomType(string roomType)
        {
            switch (roomType)
            {
                case "public": return RoomType.Public;
                default: throw new ArgumentException("Wrong roomtype!");
            }
        }
    }
}