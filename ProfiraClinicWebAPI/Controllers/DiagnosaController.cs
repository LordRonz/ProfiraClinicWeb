using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class DiagnosaController
    : BaseCrudController<TRMDiagnosa>
    {
        public class AddDiagnosaDto()
        {
            public string? KodeLokasi { get; set; }
            public DateTime? TanggalTransaksi { get; set; }
            public string? NomorAppointment { get; set; }
            public string? KodeCustomer { get; set; }
            public string? KodeKaryawan { get; set; }
            public string? KodeDiagnosa { get; set; }
            public string? KategoriDiagnosa { get; set; }
            public string? KeteranganDiagnosa { get; set; }
            public string? INPMD { get; set; }
            public string? NOFAK { get; set; }
        }

        public DiagnosaController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<TRMDiagnosa> DbSet
            => _context.TRMDiagnosa;

        protected override IQueryable<TRMDiagnosa> ApplySearch(
            IQueryable<TRMDiagnosa> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam)
            || EF.Functions.Like(d.KodeLokasi, likeParam)
            );

        protected override IOrderedQueryable<TRMDiagnosa> ApplyOrder(
            IQueryable<TRMDiagnosa> q)
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
                new SqlParameter("@INPMD", appDto.INPMD ?? (object)DBNull.Value),
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
                "@USRID, @INPMD, @NOFAK OUTPUT",
                sqlParameters
            );

            var noFak = sqlParameters.Last().Value?.ToString();

            // Return the newly created patient. Adjust properties as needed.
            var result = CreatedAtAction(nameof(GetItem),
                             new { noFak = noFak });

            return result;
        }
    }
}
