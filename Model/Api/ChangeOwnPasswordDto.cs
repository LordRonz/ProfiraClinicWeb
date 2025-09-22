namespace ProfiraClinic.Models.Api
{
    public class ChangeOwnPasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
