namespace WebChat.DataLayer.Repositories
{
    using Models;
    public class RoomsRepository : GenericRepositorty<Room>
    {
        public RoomsRepository(WebChatContext context)
            : base(context)
        { 
        }
    }
}
