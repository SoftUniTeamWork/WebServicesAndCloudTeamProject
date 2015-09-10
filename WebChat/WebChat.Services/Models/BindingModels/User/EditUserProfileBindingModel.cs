namespace WebChat.Services.Models.BindingModels.User
{
    using System.ComponentModel.DataAnnotations;

    public class EditUserProfileBindingModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}