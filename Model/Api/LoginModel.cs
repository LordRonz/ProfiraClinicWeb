namespace ProfiraClinic.Models.Api
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? KodeLokasi { get; set; }
    }
}
