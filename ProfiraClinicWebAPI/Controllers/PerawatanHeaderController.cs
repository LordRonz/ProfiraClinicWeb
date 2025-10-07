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
    public class PerawatanHeaderController
    : BaseCrudController<TRMPerawatanHeader>
    {
        public class TRMPerawatanHeaderListDto()
        {
            public string? KodeCustomer { get; set; }
        }
        private readonly string _kodePoli;

        public PerawatanHeaderController(AppDbContext ctx, IConfiguration configuration) : base(ctx)
        {
            _kodePoli = configuration["KodePoli"] ?? "";
        }

        protected override DbSet<TRMPerawatanHeader> DbSet
            => _context.TRMPerawatanHeader;

        protected override IQueryable<TRMPerawatanHeader> ApplySearch(
            IQueryable<TRMPerawatanHeader> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodePoli, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam)
            || EF.Functions.Like(d.KodeLokasi, likeParam)
            );

        protected override IOrderedQueryable<TRMPerawatanHeader> ApplyOrder(
            IQueryable<TRMPerawatanHeader> q)
            => q.OrderBy(d => d.TanggalTransaksi);

        [HttpPost("AddTRMPerawatanHeader")]
        public async Task<IActionResult> AddTRMPerawatanHeader([FromBody] AddTRMPerawatanHeaderDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser.AsNoTracking()
                .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound();

            // prefer kode lokasi from token, fallback to user, then dto
            var kdLok = User.FindFirstValue(JwtClaimTypes.KodeLokasi)
                       ?? user.KodeLokasi
                       ?? appDto.KodeLokasi;

            // @NomorTransaksi is returned by the SP via assignment; caller must pass InputOutput
            var nomorParam = new SqlParameter("@NomorTransaksi", System.Data.SqlDbType.Char, 21)
            {
                Direction = System.Data.ParameterDirection.InputOutput,
                // seed with empty char(21)
                Value = (object)string.Empty.PadRight(21, ' ')
            };

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi",    kdLok ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Today),
        new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer",  appDto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodePoli",      _kodePoli ?? (object)DBNull.Value),
        new SqlParameter("@Keterangan",    appDto.KeteranganTRMPerawatanHeader ?? (object)DBNull.Value),
        new SqlParameter("@USRID",         user.KodeUser ?? user.USRID ?? (object)DBNull.Value),
        nomorParam
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_Perawatan_Header_Add " +
                    "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, " +
                    "@KodeCustomer, @KodePoli, @Keterangan, @USRID, @NomorTransaksi",
                    sqlParameters);

                var nomorTransaksi = (nomorParam.Value as string)?.Trim();
                return Ok(new { nomorTransaksi });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { message = "Failed to add TRM Perawatan Header.", error = ex.Message });
            }
        }


        [HttpPost("EditTRMPerawatanHeader")]
        public async Task<IActionResult> EditTRMPerawatanHeader([FromBody] EditTRMPerawatanHeaderDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser.AsNoTracking()
                .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(appDto.NomorTransaksi))
                return BadRequest(new { message = "NomorTransaksi is required for editing." });

            var kdLok = User.FindFirstValue(JwtClaimTypes.KodeLokasi)
                       ?? user.KodeLokasi
                       ?? appDto.KodeLokasi;

            try
            {
                // 1) Delete existing header (Edit SP assumes nomor already removed)
                var delParams = new[] { new SqlParameter("@NomorTransaksi", appDto.NomorTransaksi) };

                // If your delete SP name is different, change it here.
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_Perawatan_Header_Del @NomorTransaksi", delParams);

                // 2) Re-insert via Edit SP
                var editParams = new[]
                {
            new SqlParameter("@KodeLokasi",    kdLok ?? (object)DBNull.Value),
            new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Today),
            new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
            new SqlParameter("@KodeCustomer",  appDto.KodeCustomer ?? (object)DBNull.Value),
            new SqlParameter("@KodePoli",      _kodePoli ?? (object)DBNull.Value),
            new SqlParameter("@Keterangan",    appDto.KeteranganTRMPerawatanHeader ?? (object)DBNull.Value),
            new SqlParameter("@USRID",         user.KodeUser ?? user.USRID ?? (object)DBNull.Value),
            new SqlParameter("@NomorTransaksi", appDto.NomorTransaksi)
        };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_Perawatan_Header_Edit " +
                    "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, " +
                    "@KodeCustomer, @KodePoli, @Keterangan, @USRID, @NomorTransaksi",
                    editParams);

                return Ok(new { nomorTransaksi = appDto.NomorTransaksi });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { message = "Failed to edit TRM Perawatan Header.", error = ex.Message });
            }
        }

        [HttpPost("GetByNomorTransaksi")]
        public async Task<IActionResult> GetByNomorTransaksi([FromBody] GetByNomorTransaksiDto dto)
        {
            var nomorTransaksi = dto.NomorTransaksi;
            if (string.IsNullOrWhiteSpace(nomorTransaksi))
                return BadRequest(new { message = "NomorTransaksi is required." });

            var diagnosa = await _context.TRMPerawatanHeader
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorTransaksi == nomorTransaksi);

            if (diagnosa == null)
                return Ok(new { message = "TRMPerawatanHeader not found." });

            return Ok(diagnosa);
        }

        [HttpPost("GetByNomorAppointment")]
        public async Task<IActionResult> GetByNomorAppointment([FromBody] GetByNomorAppointmentDto dto)
        {
            var nomorAppointment = dto.NomorAppointment;
            if (string.IsNullOrWhiteSpace(nomorAppointment))
                return BadRequest(new { message = "NomorAppointment is required." });

            var diagnosa = await _context.TRMPerawatanHeader
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorAppointment == nomorAppointment);

            return Ok(diagnosa);
        }
    }
}
