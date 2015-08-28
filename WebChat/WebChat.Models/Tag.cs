﻿namespace WebChat.Models
{
    using System.Collections.Generic;
    public class Tag
    {
        public Tag()
        {
            this.Rooms = new HashSet<Room>();
        }
        public string Name { get; set; }


        public virtual ICollection<Room> Rooms { get; set; }
    }
}