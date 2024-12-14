using Microsoft.AspNetCore.Identity;

namespace AuthenticatorBackend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}