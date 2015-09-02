using System.Web.Http;

namespace WebChat.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    [RoutePrefix("api/user")]
    public class ApplicationUser : IdentityUser
    {
        private ICollection<Message> messages;

        public ApplicationUser()
        {
            this.messages = new HashSet<Message>();
        }

        public string CurrentLocation { get; set; }

        public virtual ICollection<Message> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }

        }

        public virtual Room Room { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                this,
                authenticationType);

            return userIdentity;
        }


    }
}
