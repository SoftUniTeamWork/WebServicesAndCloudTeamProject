namespace WebChat.DataLayer.Repositories
{
    using Models;

    public class UsersRepository : GenericRepositorty<ApplicationUser>
    {
        public UsersRepository(WebChatContext context)
            : base(context)
        { 
        }
    }
}
