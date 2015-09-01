namespace WebChat.DataLayer.Repositories
{
    using Models;
    public class MessagesRepository : GenericRepositorty<Message>
    {
        public MessagesRepository(WebChatContext context) : base(context)
        { 
        }
    }
}
