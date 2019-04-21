using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Englishmania.Web.Models
{
    public class UserRegisterModel
    {
        [Required] public string Name { get; set; }
        [Required] public string Login { get; set; }
        [Required] public string PasswordHash { get; set; }
    }
}
