namespace WebChat.Models
{
    using System.Collections.Generic;

    public class Tag
    {
        private ICollection<Room> rooms;
        public Tag()
        {
            this.rooms = new HashSet<Room>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Room> Rooms
        {
            get { return this.rooms; }
            set { this.rooms = value; }
        } 
    }
}
