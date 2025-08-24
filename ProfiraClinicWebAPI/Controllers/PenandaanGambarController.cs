using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PenandaanGambarController : BaseCrudController<TRMPenandaanGambarHeader>
    {
        private readonly string _kodePoli;
        public PenandaanGambarController(AppDbContext ctx, IConfiguration configuration) : base(ctx)
        {
            _kodePoli = configuration["KodePoli"];
        }

        protected override DbSet<TRMPenandaanGambarHeader> DbSet
            => _context.TRMPenandaanGambarHeader;

        protected override IQueryable<TRMPenandaanGambarHeader> ApplySearch(
            IQueryable<TRMPenandaanGambarHeader> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan!, likeParam)
                || EF.Functions.Like(d.KodeCustomer!, likeParam));

        protected override IOrderedQueryable<TRMPenandaanGambarHeader> ApplyOrder(
            IQueryable<TRMPenandaanGambarHeader> q)
            => q.OrderBy(d => d.UPDDT);

        // ===============================
        // Penandaan Gambar Header
        // ===============================

        [HttpPost("AddPenandaanGambarHeader")]
        public async Task<IActionResult> AddPenandaanGambarHeader([FromBody] AddPenandaanGambarHeaderDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound("User not found");

            var karyawan = await _context.MKaryawan
                                .FirstOrDefaultAsync(k => k.USRID == user.KodeUser);

            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound("Karyawan not found");

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? user.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
        new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
        new SqlParameter("@KodePoli", _kodePoli ?? (object)DBNull.Value),
        new SqlParameter("@NomorUrut", appDto.NomorUrut),
        new SqlParameter("@Keterangan", appDto.Keterangan ?? (object)DBNull.Value),
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
                "EXEC dbo.usp_TRM_PenandaanGambar_Header_Add " +
                "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, @KodeCustomer, " +
                "@KodeKaryawan, @KodePoli, @NomorUrut, @Keterangan, @USRID, @NomorTransaksi OUTPUT",
                sqlParameters
            );

            var nomorTransaksi = sqlParameters.Last().Value?.ToString()?.Trim();

            return Ok(new { nomorTransaksi });
        }


        [HttpPost("EditPenandaanGambarHeader")]
        public async Task<IActionResult> EditPenandaanGambarHeader([FromBody] TRMPenandaanGambarHeader header)
        {
            if (header == null || string.IsNullOrEmpty(header.NomorTransaksi))
                return BadRequest("NomorTransaksi is required");

            var existing = await _context.TRMPenandaanGambarHeader.FindAsync(header.NomorTransaksi);
            if (existing == null) return NotFound("Header not found");

            _context.Entry(existing).CurrentValues.SetValues(header);
            existing.UPDDT = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Header updated successfully", data = existing });
        }

        // ===============================
        // Penandaan Gambar Detail
        // ===============================

        [HttpPost("AddPenandaanGambarDetail")]
        public async Task<IActionResult> AddPenandaanGambarDetail([FromBody] AddPenandaanGambarDetailDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrEmpty(dto.NomorTransaksi))
                return BadRequest("NomorTransaksi is required");

            var sqlParameters = new[]
            {
        new SqlParameter("@NomorTransaksi", dto.NomorTransaksi),
        new SqlParameter("@KodeGambar", dto.KodeGambar ?? (object)DBNull.Value),
        new SqlParameter("@IDGambar", dto.IDGambar ?? (object)DBNull.Value),
        new SqlParameter
        {
            ParameterName = "@IDDetail",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Output
        }
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_PenandaanGambar_Detail_Add " +
                "@NomorTransaksi, @KodeGambar, @IDGambar, @IDDetail OUTPUT",
                sqlParameters
            );

            var idDetail = (int?)sqlParameters.Last().Value;

            return Ok(new { idDetail });
        }


        [HttpPost("EditPenandaanGambarDetail")]
        public async Task<IActionResult> EditPenandaanGambarDetail([FromBody] TRMPenandaanGambarDetail detail)
        {
            if (detail == null || string.IsNullOrEmpty(detail.NomorTransaksi))
                return BadRequest("NomorTransaksi is required");

            var existing = await _context.TRMPenandaanGambarDetail.FindAsync(detail.NomorTransaksi);
            if (existing == null) return NotFound("Detail not found");

            _context.Entry(existing).CurrentValues.SetValues(detail);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Detail updated successfully", data = existing });
        }
    }
}
