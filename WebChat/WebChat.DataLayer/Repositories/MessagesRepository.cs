namespace WebChat.DataLayer.Repositories
{
    using Models;
    class MessagesRepository : GenericRepositorty<Message>
    {
        public MessagesRepository(WebChatContext context) : base(context)
        { 
        }
    }
}
