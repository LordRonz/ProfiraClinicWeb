
using ProfiraClinicRME.Utils;

namespace ProfiraClinicRME.Helpers
{



    public static class UIHelper
    {

        public static string GetJenisKelamin(string kode)
        {
            return kode == "0" ? "Laki-laki" : "Perempuan";
        }

        //appointment

        /// <summary>
        /// get status appointment from status code
        /// </summary>
        /// <param name="kode"></param>
        /// <returns></returns>
        public static string GetStatusAppointment(string kode)
        {
            return kode switch
            {
                "1" => "App",
                "2" => "Konfirmasi",
                "3" => "Arrival",
                "4" => "Cancel",
                "5" => "Reschedule",
                "6" => "Walk-in",
                _ => "Tidak Diketahui"
            };
        }

        /// <summary>
        /// get status tindakan from status code
        /// </summary>
        /// <param name="kode"></param>
        /// <returns></returns>
        public static string GetStatusTindakan(string? kode)
        {
            return kode switch
            {
                "1" => "Waiting List",
                "2" => "On Progress",
                "3" => "Confirm",
                "4" => "Billing",
                _ => "Tidak Diketahui"
            };
        }
    }

}