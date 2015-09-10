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
                case "Public": return RoomType.Public;
                case "Private": return RoomType.Private;
                default: throw new ArgumentException("Wrong roomtype!");
            }
        }
    }
}