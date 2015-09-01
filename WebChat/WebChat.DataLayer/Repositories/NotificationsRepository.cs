namespace WebChat.DataLayer.Repositories
{
    using Models;
    public class NotificationsRepository: GenericRepositorty<Notification>
    {
        public NotificationsRepository(WebChatContext context)
            : base(context)
        { 
        }
    }
}
