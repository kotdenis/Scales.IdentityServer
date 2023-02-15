using System.ComponentModel.DataAnnotations;

namespace Scales.IdentityServer.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; } = string.Empty;
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
