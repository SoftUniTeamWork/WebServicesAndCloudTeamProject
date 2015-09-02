using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebChat.Services.Models.BindingModels.User
{
    public class UserCreateMessageBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}