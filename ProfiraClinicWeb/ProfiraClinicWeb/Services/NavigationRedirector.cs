using System;

namespace ProfiraClinicWeb.Services
{
    public class NavigationRedirector : INavigationRedirector
    {
        public bool ShouldRedirect { get; set; }
        public string? TargetUrl { get; set; }
    }
}