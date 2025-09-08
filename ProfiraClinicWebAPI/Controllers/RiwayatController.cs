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
    public class RiwayatController
    : BaseCrudController<Riwayat>
    {
        public class RiwayatListDto()
        {
            public string? KodeCustomer { get; set; }
        }

        private readonly string _kodePoli;
        public RiwayatController(AppDbContext ctx, IConfiguration configuration) : base(ctx)
        {
            _kodePoli = configuration["KodePoli"];
        }

        protected override DbSet<Riwayat> DbSet
            => _context.Riwayat;

        protected override IQueryable<Riwayat> ApplySearch(
            IQueryable<Riwayat> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam)
            || EF.Functions.Like(d.KodeLokasi, likeParam)
            );

        protected override IOrderedQueryable<Riwayat> ApplyOrder(
            IQueryable<Riwayat> q)
            => q.OrderBy(d => d.TanggalTransaksi);

        [HttpPost("AddRiwayat")]
        public async Task<IActionResult> AddRiwayat([FromBody] AddRiwayatDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // Look up user
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound();

            var kdLok = User.FindFirstValue(JwtClaimTypes.KodeLokasi);

            var karyawan = await _context.MKaryawan
                            .FirstOrDefaultAsync(k => k.USRID == user.KodeUser);

            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? kdLok ?? user.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
        new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
        new SqlParameter("@KodePoli", _kodePoli ?? (object)DBNull.Value),
        new SqlParameter("@PenyakitDahulu", appDto.PenyakitDahulu ?? (object)DBNull.Value),
        new SqlParameter("@chkPenyakit", appDto.chkPenyakit ?? (object)DBNull.Value),
        new SqlParameter("@PenyakitSekarang", appDto.PenyakitSekarang ?? (object)DBNull.Value),
        new SqlParameter("@chkAlergiObat", appDto.chkAlergiObat ?? (object)DBNull.Value),
        new SqlParameter("@KetAlergiObat", appDto.KetAlergiObat ?? (object)DBNull.Value),
        new SqlParameter("@chkAlergiMakanan", appDto.chkAlergiMakanan ?? (object)DBNull.Value),
        new SqlParameter("@KetAlergiMakanan", appDto.KetAlergiMakanan ?? (object)DBNull.Value),
        new SqlParameter("@chkResiko", appDto.chkResiko ?? (object)DBNull.Value),
        new SqlParameter("@KetResiko", appDto.KetResiko ?? (object)DBNull.Value),
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
                "EXEC dbo.usp_TRM_Riwayat_Add " +
                "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, " +
                "@KodeCustomer, @KodeKaryawan, @KodePoli, " +
                "@PenyakitDahulu, @chkPenyakit, @PenyakitSekarang, " +
                "@chkAlergiObat, @KetAlergiObat, @chkAlergiMakanan, @KetAlergiMakanan, " +
                "@chkResiko, @KetResiko, @USRID, @NomorTransaksi OUTPUT",
                sqlParameters
            );

            var nomorTransaksi = sqlParameters.Last().Value?.ToString()?.Trim();

            return Ok(new { nomorTransaksi });
        }


        [HttpPost("EditRiwayat")]
        public async Task<IActionResult> EditRiwayat([FromBody] EditRiwayatDto appDto)
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

            var karyawan = await _context.MKaryawan
                .FirstOrDefaultAsync(k => k.USRID == user.KodeUser);

            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            var delParam = new[]
            {
        new SqlParameter("@NomorTransaksi", appDto.NomorTransaksi)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_Riwayat_Del @NomorTransaksi",
                delParam
            );

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? kdLok ?? user.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
        new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
        new SqlParameter("@PenyakitDahulu", appDto.PenyakitDahulu ?? (object)DBNull.Value),
        new SqlParameter("@chkPenyakit", appDto.chkPenyakit ?? (object)DBNull.Value),
        new SqlParameter("@PenyakitSekarang", appDto.PenyakitSekarang ?? (object)DBNull.Value),
        new SqlParameter("@chkAlergiObat", appDto.chkAlergiObat ?? (object)DBNull.Value),
        new SqlParameter("@KetAlergiObat", appDto.KetAlergiObat ?? (object)DBNull.Value),
        new SqlParameter("@chkAlergiMakanan", appDto.chkAlergiMakanan ?? (object)DBNull.Value),
        new SqlParameter("@KetAlergiMakanan", appDto.KetAlergiMakanan ?? (object)DBNull.Value),
        new SqlParameter("@chkResiko", appDto.chkResiko ?? (object)DBNull.Value),
        new SqlParameter("@KetResiko", appDto.KetResiko ?? (object)DBNull.Value),
        new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
        new SqlParameter("@NomorTransaksi", appDto.NomorTransaksi ?? (object)DBNull.Value)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_Riwayat_Edit " +
                "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, " +
                "@KodeCustomer, @KodeKaryawan, " +
                "@PenyakitDahulu, @chkPenyakit, @PenyakitSekarang, " +
                "@chkAlergiObat, @KetAlergiObat, @chkAlergiMakanan, @KetAlergiMakanan, " +
                "@chkResiko, @KetResiko, @USRID, @NomorTransaksi",
                sqlParameters
            );

            return Ok(new { message = "Riwayat updated successfully" });
        }




        [HttpPost("GetListTrm")]
        public async Task<IActionResult> GetListTrm([FromBody] RiwayatListDto diagDto)
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

            var list = await _context.TRMRiwayat
                .FromSqlRaw("EXEC dbo.usp_TRM_Riwayat_List @KodeCustomer", sqlParameters)
                .ToListAsync();

            var result = new Pagination<TRMRiwayat>
            {
                TotalCount = 0,
                Page = 0,
                PageSize = 0,
                Items = list
            };

            return Ok(result);
        }

        [HttpPost("GetByNomorTransaksi")]
        public async Task<IActionResult> GetByNomorTransaksi([FromBody] GetByNomorTransaksiDto dto)
        {
            var nomorTransaksi = dto.NomorTransaksi;
            if (string.IsNullOrWhiteSpace(nomorTransaksi))
                return BadRequest(new { message = "NomorTransaksi is required." });

            var riwayat = await _context.Riwayat
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorTransaksi == nomorTransaksi);

            return Ok(riwayat);
        }

        [HttpPost("GetByNomorAppointment")]
        public async Task<IActionResult> GetByNomorAppointment([FromBody] GetByNomorAppointmentDto dto)
        {
            var nomorAppointment = dto.NomorAppointment;
            if (string.IsNullOrWhiteSpace(nomorAppointment))
                return BadRequest(new { message = "NomorAppointment is required." });

            var riwayat = await _context.Riwayat
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorAppointment == nomorAppointment);

            return Ok(riwayat);
        }
    }
}
