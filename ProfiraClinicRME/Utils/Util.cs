using System.Text.Json;

namespace ProfiraClinicRME.Utils
{
    public class OpStatus
    {
        public int Status { get; set; } = 1;
        public string Message { get; set; } = "";

        public object Data { get; set; } // Use JsonElement for manual parsing

    }

    public static class JsInvoke
    {

    }

    public class Util
    {
        public static string GenerateRandomAlphanumeric(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
