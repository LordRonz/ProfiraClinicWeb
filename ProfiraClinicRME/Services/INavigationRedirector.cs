namespace ProfiraClinicRME.Services
{
    public interface INavigationRedirector
    {
        bool ShouldRedirect { get; set; }
        string? TargetUrl { get; set; }
    }
}
