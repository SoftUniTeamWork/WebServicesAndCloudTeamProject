namespace WebChat.DataLayer.Repositories
{
    using Models;

    class UsersRepository : GenericRepositorty<ApplicationUser>
    {
        public UsersRepository(WebChatContext context)
            : base(context)
        { 
        }
    }
}
