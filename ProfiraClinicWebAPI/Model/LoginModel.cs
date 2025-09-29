namespace ProfiraClinicWebAPI.Model
{
    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? KodeLokasi { get; set; }
        public string Role { get; set; } = "User";
    }
}
