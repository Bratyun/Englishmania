﻿using System.ComponentModel.DataAnnotations;

namespace Englishmania.Web.Models.User
{
    public class UserRegisterModel
    {
        [Required] public string Name { get; set; }
        [Required] public string Login { get; set; }
        [Required] public string PasswordHash { get; set; }
    }
}