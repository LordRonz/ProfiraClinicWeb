using ProfiraClinic.Models.Core;

namespace ProfiraClinicRME.Model
{
    public class RiwayatAdaptor
    {
        public TRMRiwayat Entity { get; init; }

        public string NomorTransaksi
        {
            get => Entity.NomorTransaksi;
            set => Entity.NomorTransaksi = value;
        }

        public string TRCD
        {
            get => Entity.TRCD;
            set => Entity.TRCD = value;
        }

        public string TRSC
        {
            get => Entity.TRSC;
            set => Entity.TRSC = value;
        }

        public string KodeLokasi
        {
            get => Entity.KodeLokasi;
            set => Entity.KodeLokasi = value;
        }

        public string KetLokasi
        {
            get => "n/a";
            set => _ = value;
        }

        public string TahunTransaksi
        {
            get => Entity.TahunTransaksi;
            set => Entity.TahunTransaksi = value;
        }

        public string BulanTransaksi
        {
            get => Entity.BulanTransaksi;
            set => Entity.BulanTransaksi = value;
        }

        public DateTime TanggalTransaksi
        {
            get => Entity.TanggalTransaksi;
            set => Entity.TanggalTransaksi = value;
        }

        public string NomorAppointment
        {
            get => Entity.NomorAppointment;
            set => Entity.NomorAppointment = value;
        }

        public string KodeCustomer
        {
            get => Entity.KodeCustomer;
            set => Entity.KodeCustomer = value;
        }

        public string KodeKaryawan
        {
            get => Entity.KodeKaryawan;
            set => Entity.KodeKaryawan = value;
        }


        public string PenyakitDahulu
        {
            get => Entity.PenyakitDahulu;
            set => Entity.PenyakitDahulu = value;
        }


        public bool IsDM
        {
            get => Entity.chkPenyakit[0] == '1' ? true : false;
            set => StoreChkPenyakit(0, value);
        }

        public bool IsHipertensi
        {
            get => Entity.chkPenyakit[1] == '1' ? true : false;
            set => StoreChkPenyakit(1, value);
        }

        public bool IsTBC
        {
            get => Entity.chkPenyakit[2] == '1' ? true : false;
            set => StoreChkPenyakit(2, value);
        }

        public bool IsAsthma
        {
            get => Entity.chkPenyakit[3] == '1' ? true : false;
            set => StoreChkPenyakit(3, value);
        }

        public bool IsHepatitis
        {
            get => Entity.chkPenyakit[4] == '1' ? true : false;
            set => StoreChkPenyakit(4, value);
        }

        public bool IsKelainanDarah
        {
            get => Entity.chkPenyakit[5] == '1' ? true : false;
            set => StoreChkPenyakit(5, value);
        }

        public bool IsPanyakitLain
        {
            get => Entity.chkPenyakit[6] == '1' ? true : false;
        }

        public string PenyakitSekarang
        {
            get => Entity.PenyakitSekarang;
            set {
                Entity.PenyakitSekarang = value;
                if (Entity.PenyakitSekarang.Length > 0)
                    StoreChkPenyakit(6, true);
                else
                    StoreChkPenyakit(6, false);
            }
        }

        public string DisplayPenyakitSekarang { 
            get{
                var allText = IsDM ? "DM, " : "";
                allText += IsHipertensi ? "Hipertensi, " : "";
                allText += IsTBC ? "TBC, " : "";
                allText += IsAsthma ? "Asma, " : "";
                allText += IsHepatitis ? "Hepatitis, " : "";
                allText += IsKelainanDarah ? "Kelainan Darah, " : "";
                allText += PenyakitSekarang;
                return allText.Length > 0 ? allText : "tidak ada";
            } 
        }

        public string DisplayResiko
        {
            get
            {
                var allText = IsMerokok ? "Merokok, " : "";
                allText += IsObesitas ? "Obesitas, " : "";
                allText += IsDisplidemia ? "Displidemia, " : "";
                allText += KetResiko;
                return allText.Length > 0 ? allText : "tidak ada";
            }
        }

        public bool IsAlergiObat
        {
            get => Entity.KetAlergiObat.Length > 0 ? true : false;
        }

        public string KetAlergiObat
        {
            get => Entity.KetAlergiObat;
            set
            {
                Entity.KetAlergiObat = value;
                Entity.chkAlergiObat = value.Length > 0 ? "1" : "0";
            }
        }

        public bool IsAlergiMakanan
        {
            get => Entity.KetAlergiMakanan.Length > 0 ? true : false;
        }

        public string KetAlergiMakanan
        {
            get => Entity.KetAlergiMakanan;
            set
            {
                Entity.KetAlergiMakanan = value;
                Entity.chkAlergiMakanan = value.Length > 0 ? "1" : "0";
            }
        }


        public bool IsMerokok
        {
            get => Entity.chkResiko[0] == '1' ? true : false;
            set => StoreChkResiko(0, value);
        }

        public bool IsObesitas
        {
            get => Entity.chkResiko[1] == '1' ? true : false;
            set => StoreChkResiko(1, value);
        }

        public bool IsDisplidemia
        {
            get => Entity.chkResiko[2] == '1' ? true : false;
            set => StoreChkResiko(2, value);
        }

        public bool IsResikoLain
        {
            get => Entity.KetResiko.Length > 0 ? true : false;
        }

        public string KetResiko
        {
            get => Entity.KetResiko;
            set
            {
                Entity.KetResiko = value;
                if (Entity.KetResiko.Length > 0)
                    StoreChkResiko(3, true);
                else
                    StoreChkResiko(3, false);
            }
        }

        public DateTime? UPDDT
        {
            get => Entity.UPDDT;
            set => Entity.UPDDT = value;
        }

        public string USRID
        {
            get => Entity.USRID;
            set => Entity.USRID = value;
        }

        public string? NamaCustomer
        {
            get => Entity.NamaCustomer;
            set => Entity.NamaCustomer = value;
        }

        public string? NamaKaryawan
        {
            get => Entity.NamaKaryawan;
            set => Entity.NamaKaryawan = value;
        }

        public RiwayatAdaptor()
        {
            Entity = new TRMRiwayat();
        }

        public RiwayatAdaptor(TRMRiwayat entity)
        {
            Entity = entity;
        }

        private void StoreChkPenyakit(int idxToReplace, bool value)
        {
            char[] charArray = Entity.chkPenyakit.ToCharArray();
            charArray[idxToReplace] = value ? '1' : '0';
            Entity.chkPenyakit = new string(charArray);
        }

        private void StoreChkResiko(int idxToReplace, bool value)
        {
            char[] charArray = Entity.chkResiko.ToCharArray();
            charArray[idxToReplace] = value ? '1' : '0';
            Entity.chkResiko = new string(charArray);
        }
    }
}
