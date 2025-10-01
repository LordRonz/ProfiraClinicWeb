namespace ProfiraClinicRME.Utils
{
    public static class DateUtility
    {
        public static int GetYearDiff(DateTime earlier, DateTime later)
        {
            return GetDateDiff(earlier, later).Item1;
        }

        public static (int, int, int) GetDateDiff(DateTime earlier, DateTime later)
        {
            DateTime zeroTime = new(1, 1, 1);

            TimeSpan span = later - earlier;
            // Because we start at year 1 for the Gregorian
            // calendar, we must subtract a year here.
            int years = (zeroTime + span).Year - 1;
            int months = (zeroTime + span).Month - 1;
            int days = (zeroTime + span).Day;

            return (years, months, days);
        }

        public enum FormatMode
        {
            DateTime,
            Date,
            Time
        }

        public static string FormatDate(DateTime? dtSource, FormatMode mode=FormatMode.Date)
        {
            if (dtSource is null) return "-";
            string format = "";
            switch (mode)
            {
                case FormatMode.DateTime:
                    format = "dd'/'MM'/'yyyy hh':'mm tt";
                    break;
                case FormatMode.Date:
                    format = "dd'/'MM'/'yyyy";
                    break;
                default:
                    format = "hh':'mm tt";
                    break;
            }

            var x =  ((DateTime)dtSource).ToString(format);
            return x;
        }

        public static string FormatTime(TimeSpan? dtSource)
        {
            if (dtSource is null) return "-";
            DateTime container = DateTime.Today.Add(dtSource.Value);
            return container.ToString("hh':'mm tt");
        }

        public static string FormatAge(DateTime? earlier, DateTime later)
        {
            if (earlier == null) return "N/A";
            int years;
            int months;
            int days;
            (years, months, days) = DateUtility.GetDateDiff((DateTime)earlier, later);
            return $"{years} tahun {months} bulan {days} hari";
        }
    }
}
