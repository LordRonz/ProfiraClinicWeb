namespace ProfiraClinicWebAPI.Helper
{
    public static class ConfigurationHelper
    {
        public static IConfiguration? Configuration { get; set; }

        public static string ClientSecret
        {
            get => Configuration?["SatuSehatLink:ClientSecret"] ?? "";
        }

        public static string BaseUrl
        {
            get => Configuration?["SatuSehatLink:SatuSehatBaseUrl"] ?? "";
        }

        public static string AuthUrl
        {
            get => Configuration?["SatuSehatLink:SatuSehatAuthUrl"] ?? "";
        }

        public static string JwtSecret
        {
            get => Configuration?["Auth:JwtSecret"] ?? "";
        }

        public static string ClientId
        {
            get => Configuration?["SatuSehatLink:ClientId"] ?? "";
        }

        public static string OrgId
        {
            get => Configuration?["SatuSehatLink:OrgId"] ?? "";
        }

        public static string AuthUser
        {
            get => Configuration?["Auth:User"] ?? "";
        }
        public static string AuthPass
        {
            get => Configuration?["Auth:Pass"] ?? "";
        }
    }
}
