namespace ProfiraClinicRME.Utils
{
    public static class Date
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
    }
}
