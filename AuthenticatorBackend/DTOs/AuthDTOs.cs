namespace AuthenticatorBackend.DTOs
{
    public class RegisterRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class ResetPasswordRequest
    {
        public required string Username { get; set; }
        public required string NewPassword { get; set; }
    }
}