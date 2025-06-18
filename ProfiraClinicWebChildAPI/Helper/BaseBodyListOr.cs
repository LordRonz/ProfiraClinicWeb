using System.Text.RegularExpressions;

namespace ProfiraClinicWebChildAPI.Helper
{
    public partial class BaseBodyListOr
    {
        private static readonly Regex _multiSpace = MyRegex();

        private string _param = "%";
        public string Param
        {
            get => _param;
            set => _param = string.IsNullOrWhiteSpace(value) ? "%" : value;
        }

        public string GetParam
        {
            get
            {
                // 1) collapse runs of whitespace to one space
                var cleaned = _multiSpace.Replace(_param, " ").Trim();

                // 2) replace any quotes with spaces, then collapse again
                cleaned = _multiSpace.Replace(cleaned, " ");

                // if it's just wildcard, leave it
                cleaned = cleaned.Replace(" ", "%");
                if (cleaned == "%") return "%";

                cleaned = cleaned.Replace("\"", " ");

                // 3) replace each remaining space with '%' and wrap
                var pattern = "%" + cleaned + "%";
                return pattern;
            }
        }

        [GeneratedRegex(@"\s+", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
    }
}
