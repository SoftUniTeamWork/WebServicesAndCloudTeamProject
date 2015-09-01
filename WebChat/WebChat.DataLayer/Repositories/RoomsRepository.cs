namespace WebChat.DataLayer.Repositories
{
    using Models;
    class RoomsRepository : GenericRepositorty<Room>
    {
        public RoomsRepository(WebChatContext context)
            : base(context)
        { 
        }
    }
}
