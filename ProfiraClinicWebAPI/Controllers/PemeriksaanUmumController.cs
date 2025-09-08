using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Services;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class PemeriksaanUmumController
     : BaseCrudController<PemeriksaanUmum>
    {
        public class AddPemeriksaanUmumDto
        {
            public string? KodeLokasi { get; set; }
            public DateTime? TanggalTransaksi { get; set; }
            public string? NomorAppointment { get; set; }
            public string? KodeCustomer { get; set; }
            public string? KodeKaryawan { get; set; }
            public string? Keadaan_Umum { get; set; }
            public string? Tingkat_Kesadaran { get; set; }
            public int? Sistolik { get; set; }
            public int? Distolik { get; set; }
            public decimal? Suhu { get; set; }
            public decimal? Saturasi { get; set; }
            public int? Frekuensi_Nadi { get; set; }
            public decimal? Frekuensi_Nafas { get; set; }
            public decimal? BeratBadan { get; set; }
            public decimal? TinggiBadan { get; set; }
            public int? IndexTubuh { get; set; }
            public decimal? LingkarKepala { get; set; }
        }


        public class PemeriksaanUmumListDto()
        {
            public string? KodeCustomer { get; set; }
        }

        private readonly string _kodePoli;
        public PemeriksaanUmumController(AppDbContext ctx, IConfiguration configuration) : base(ctx)
        {
            _kodePoli = configuration["KodePoli"] ?? "";
        }

        protected override DbSet<PemeriksaanUmum> DbSet
            => _context.PemeriksaanUmum;

        protected override IQueryable<PemeriksaanUmum> ApplySearch(
            IQueryable<PemeriksaanUmum> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam)
            || EF.Functions.Like(d.KodeLokasi, likeParam)
            );

        protected override IOrderedQueryable<PemeriksaanUmum> ApplyOrder(
            IQueryable<PemeriksaanUmum> q)
            => q.OrderBy(d => d.TanggalTransaksi);

        [HttpPost("AddPemeriksaanUmum")]
        public async Task<IActionResult> AddPemeriksaanUmum([FromBody] AddPemeriksaanUmumDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName)) return Unauthorized();

            var user = await _context.MUser.AsNoTracking().FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null) return NotFound();

            var kdLok = User.FindFirstValue(JwtClaimTypes.KodeLokasi);

            var karyawan = await _context.MKaryawan.FirstOrDefaultAsync(k => k.USRID == user.KodeUser);
            if (appDto.KodeKaryawan == null && karyawan == null) return NotFound();

            var sqlParameters = new[]
                    {
                new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? kdLok ?? user.KodeLokasi ?? (object)DBNull.Value),
                new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
                new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
                new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
                new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
                new SqlParameter("@KodePoli", _kodePoli ?? (object)DBNull.Value),
                new SqlParameter("@Keadaan_Umum", appDto.Keadaan_Umum ?? (object)DBNull.Value),
                new SqlParameter("@Tingkat_Kesadaran", appDto.Tingkat_Kesadaran ?? (object)DBNull.Value),
                new SqlParameter("@Sistolik", appDto.Sistolik ?? (object)DBNull.Value),
                new SqlParameter("@Distolik", appDto.Distolik ?? (object)DBNull.Value),
                new SqlParameter("@Suhu", appDto.Suhu ?? (object)DBNull.Value),
                new SqlParameter("@Saturasi", appDto.Saturasi ?? (object)DBNull.Value),
                new SqlParameter("@Frekuensi_Nadi", appDto.Frekuensi_Nadi ?? (object)DBNull.Value),
                new SqlParameter("@Frekuensi_Nafas", appDto.Frekuensi_Nafas ?? (object)DBNull.Value),
                new SqlParameter("@BeratBadan", appDto.BeratBadan ?? (object)DBNull.Value),
                new SqlParameter("@TinggiBadan", appDto.TinggiBadan ?? (object)DBNull.Value),
                new SqlParameter("@IndexTubuh", appDto.IndexTubuh ?? (object)DBNull.Value),
                new SqlParameter("@LingkarKepala", appDto.LingkarKepala ?? (object)DBNull.Value),
                new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
                new SqlParameter
                {
                    ParameterName = "@NomorTransaksi",
                    SqlDbType = System.Data.SqlDbType.Char,
                    Size = 25,
                    Direction = System.Data.ParameterDirection.Output
                }
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_PemeriksaanUmum_Add " +
                "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, @KodeCustomer, @KodeKaryawan, @KodePoli, " +
                "@Keadaan_Umum, @Tingkat_Kesadaran, @Sistolik, @Distolik, @Suhu, @Saturasi, " +
                "@Frekuensi_Nadi, @Frekuensi_Nafas, @BeratBadan, @TinggiBadan, @IndexTubuh, @LingkarKepala, @USRID, @NomorTransaksi OUTPUT",
                sqlParameters
            );

            var nomorTransaksi = sqlParameters.Last().Value?.ToString()?.Trim();

            return Ok(new { nomorTransaksi });
        }

        [HttpPost("EditPemeriksaanUmum")]
        public async Task<IActionResult> EditPemeriksaanUmum([FromBody] EditPemeriksaanUmumDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound();

            var kdLok = User.FindFirstValue(JwtClaimTypes.KodeLokasi);

            var karyawan = await _context.MKaryawan.FirstOrDefaultAsync(k => k.USRID == user.KodeUser);

            if (dto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            // 1. Delete existing record
            var delParams = new[]
            {
        new SqlParameter("@NomorTransaksi", dto.NomorTransaksi ?? (object)DBNull.Value)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_PemeriksaanUmum_Del @NomorTransaksi",
                delParams
            );

            // 2. Insert new record (Edit = Delete + Insert)
            var editParams = new[]
            {
        new SqlParameter("@KodeLokasi", dto.KodeLokasi ?? kdLok ?? user.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", dto.TanggalTransaksi ?? DateTime.Now),
        new SqlParameter("@NomorAppointment", dto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer", dto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodeKaryawan", dto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
        new SqlParameter("@KodePoli", _kodePoli ?? (object)DBNull.Value),
        new SqlParameter("@Keadaan_Umum", dto.Keadaan_Umum ?? (object)DBNull.Value),
        new SqlParameter("@Tingkat_Kesadaran", dto.Tingkat_Kesadaran ?? (object)DBNull.Value),
        new SqlParameter("@Sistolik", dto.Sistolik),
        new SqlParameter("@Distolik", dto.Distolik),
        new SqlParameter("@Suhu", dto.Suhu),
        new SqlParameter("@Saturasi", dto.Saturasi),
        new SqlParameter("@Frekuensi_Nadi", dto.Frekuensi_Nadi),
        new SqlParameter("@Frekuensi_Nafas", dto.Frekuensi_Nafas),
        new SqlParameter("@BeratBadan", dto.BeratBadan),
        new SqlParameter("@TinggiBadan", dto.TinggiBadan),
        new SqlParameter("@IndexTubuh", dto.IndexTubuh),
        new SqlParameter("@LingkarKepala", dto.LingkarKepala),
        new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
        new SqlParameter("@NomorTransaksi", dto.NomorTransaksi ?? (object)DBNull.Value)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_PemeriksaanUmum_Edit @KodeLokasi, @TanggalTransaksi, @NomorAppointment, " +
                "@KodeCustomer, @KodeKaryawan, @KodePoli, @Keadaan_Umum, @Tingkat_Kesadaran, @Sistolik, @Distolik, " +
                "@Suhu, @Saturasi, @Frekuensi_Nadi, @Frekuensi_Nafas, @BeratBadan, @TinggiBadan, " +
                "@IndexTubuh, @LingkarKepala, @USRID, @NomorTransaksi",
                editParams
            );

            return Ok(new { nomorTransaksi = dto.NomorTransaksi });
        }



        [HttpPost("GetListTrm")]
        public async Task<IActionResult> GetListTrm([FromBody] PemeriksaanUmumListDto diagDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // 2) look it up
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeCustomer",  diagDto.KodeCustomer ?? (object)DBNull.Value),
            };

            var list = await _context.TRMPemeriksaanUmum
                .FromSqlRaw("EXEC dbo.usp_TRM_PemeriksaanUmum_List @KodeCustomer", sqlParameters)
                .ToListAsync();

            var result = new Pagination<TRMPemeriksaanUmum>
            {
                TotalCount = 0,
                Page = 0,
                PageSize = 0,
                Items = list
            };

            return Ok(result);
        }

        [HttpPost("GetByNomorAppointment")]
        public async Task<IActionResult> GetByNomorAppointment([FromBody] GetByNomorAppointmentDto dto)
        {
            var nomorAppointment = dto.NomorAppointment;
            if (string.IsNullOrWhiteSpace(nomorAppointment))
                return BadRequest(new { message = "NomorAppointment is required." });

            var result = await _context.PemeriksaanUmum
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorAppointment == nomorAppointment);

            return Ok(result);
        }
    }
}
