using Project.Models.Table;
using System.ComponentModel.DataAnnotations;

namespace Project.Models.Account
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public string VerificationToken { get; set; } = string.Empty;
        public string JwtToken { get; set; } = string.Empty;
        public List<UserTable> UserTables { get; set; } = [];



    }
}
