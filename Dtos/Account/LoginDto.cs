using System.ComponentModel.DataAnnotations;

namespace Project.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string JwtToken { get; set; } = string.Empty;
 

    }
}
