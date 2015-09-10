using System.Collections.Generic;

namespace WebChat.Services.Models.ViewModels
{
    // Models returned by AccountController actions.

    public class UserProfileViewModel
    {
        public object Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }

    
}
