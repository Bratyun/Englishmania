using System.ComponentModel.DataAnnotations;

namespace Englishmania.Web.Models
{
    public class LoginRequestModel
    {
        [Required] public string Login { get; set; }
        [Required] public string PasswordHash { get; set; }
    }
}