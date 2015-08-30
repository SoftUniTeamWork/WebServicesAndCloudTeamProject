namespace WebChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Messages = new HashSet<Message>();
            this.Notifications = new HashSet<Notification>();
        }

        public string CurrentLocation { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public ICollection<Notification> Notifications { get; set; }

        [Required]
        public int RoomId { get; set; }

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
