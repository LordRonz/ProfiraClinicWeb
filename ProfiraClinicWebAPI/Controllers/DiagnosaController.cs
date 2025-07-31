using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class DiagnosaController
    : BaseCrudController<Diagnosa>
    {
        public class DiagnosaListDto()
        {
            public string? KodeCustomer { get; set; }
        }

        public DiagnosaController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<Diagnosa> DbSet
            => _context.Diagnosa;

        protected override IQueryable<Diagnosa> ApplySearch(
            IQueryable<Diagnosa> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam)
            || EF.Functions.Like(d.KodeLokasi, likeParam)
            );

        protected override IOrderedQueryable<Diagnosa> ApplyOrder(
            IQueryable<Diagnosa> q)
            => q.OrderBy(d => d.TanggalTransaksi);

        [HttpPost("AddDiagnosa")]
        public async Task<IActionResult> AddDiagnosa([FromBody] AddDiagnosaDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // 2) look it up
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return NotFound();

            var karyawan = await _context.MKaryawan.FirstOrDefaultAsync(k => k.USRID == user.UserID);

            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? user.KodeLokasi ?? (object)DBNull.Value),
                new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
                new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
                new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
                new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
                new SqlParameter("@KodeDiagnosa", appDto.KodeDiagnosa ?? (object)DBNull.Value),
                new SqlParameter("@KategoriDiagnosa", appDto.KategoriDiagnosa ?? (object)DBNull.Value),
                new SqlParameter("@KeteranganDiagnosa", appDto.KeteranganDiagnosa ?? (object)DBNull.Value),
                new SqlParameter("@USRID", user.UserID ?? (object)DBNull.Value),
                new SqlParameter
                {
                    ParameterName = "@NOFAK",
                    SqlDbType = System.Data.SqlDbType.Char,
                    Size = 25,
                    Direction = System.Data.ParameterDirection.Output
                }
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_Diagnosa_Add @KodeLokasi, @TanggalTransaksi, @NomorAppointment, " +
                "@KodeCustomer, @KodeKaryawan, @KodeDiagnosa, @KategoriDiagnosa, @KeteranganDiagnosa, " +
                "@USRID, @NOFAK OUTPUT",
                sqlParameters
            );

            var nomorTransaksi = sqlParameters.Last().Value?.ToString()?.Trim();

            // Return the newly created patient. Adjust properties as needed.
            var result = Ok(new { nomorTransaksi = nomorTransaksi });

            return result;
        }

        [HttpPost("GetListTrm")]
        public async Task<IActionResult> GetListTrm([FromBody] DiagnosaListDto diagDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // 2) look it up
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.UserName == userName);

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeCustomer",  diagDto.KodeCustomer ?? (object)DBNull.Value),
            };

            var list = await _context.TRMDiagnosa
                .FromSqlRaw("EXEC dbo.usp_TRM_Diagnosa_List @KodeCustomer", sqlParameters)
                .ToListAsync();

            var result = new Pagination<TRMDiagnosa>
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

            var diagnosa = await _context.Diagnosa
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorTransaksi == nomorTransaksi);

            if (diagnosa == null)
                return Ok(new { message = "Diagnosa not found." });

            return Ok(diagnosa);
        }

        [HttpPost("GetByNomorAppointment")]
        public async Task<IActionResult> GetByNomorAppointment([FromBody] GetByNomorAppointmentDto dto)
        {
            var nomorAppointment = dto.NomorAppointment;
            if (string.IsNullOrWhiteSpace(nomorAppointment))
                return BadRequest(new { message = "NomorAppointment is required." });

            var diagnosa = await _context.Diagnosa
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorAppointment == nomorAppointment);

            if (diagnosa == null)
                return Ok(new { message = "Diagnosa not found." });

            return Ok(diagnosa);
        }
    }
}
