using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Services;
using System.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class AppointmentController
    : BaseCrudController<Appointment>
    {

        public class AppointmentListDto()
        {
            public string? KodeLokasi { get; set; }
            public DateTime? TanggalAppointment { get; set; }
            public string? KodeKaryawan { get; set; }
        }

        public class EditStatusTindakanDto()
        {
            public string NomorAppointment { get; set; }
            public string Status { get; set; }
        }

        public AppointmentController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<Appointment> DbSet
            => _context.Appointment;

        protected override IQueryable<Appointment> ApplySearch(
            IQueryable<Appointment> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam)
            || EF.Functions.Like(d.KodeLokasi, likeParam)
            || EF.Functions.Like(d.KodeRuangan, likeParam)
            );

        protected override IOrderedQueryable<Appointment> ApplyOrder(
            IQueryable<Appointment> q)
            => q.OrderBy(d => d.TanggalAppointment);

        [HttpPost("GetListDokter")]
        public async Task<IActionResult> GetListDokter([FromBody] AppointmentListDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // 2) look it up
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound();

            var kdLok = User.FindFirstValue(JwtClaimTypes.KodeLokasi);


            var karyawan = await _context.MKaryawan.FirstOrDefaultAsync(k => k.USRID == user.KodeUser);

            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeLokasi",  appDto.KodeLokasi ?? kdLok ?? user.KodeLokasi ?? (object)DBNull.Value),

                new SqlParameter("@TanggalAppointment", appDto.TanggalAppointment ?? (object)DBNull.Value),

                new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
            };

            var list = await _context.AppointmentList
                .FromSqlRaw("EXEC dbo.usp_TRM_Appointment_List @KodeLokasi, @TanggalAppointment, @KodeKaryawan", sqlParameters)
                .ToListAsync();

            var result = new Pagination<TRMAppointment>
            {
                TotalCount = 0,
                Page = 0,
                PageSize = 0,
                Items = list
            };

            return Ok(result);
        }

        [HttpPost("EditStatusTindakan")]
        public async Task<IActionResult> EditStatusTindakan([FromBody] EditStatusTindakanDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // Look up the user
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound();

            // Validate required fields
            if (string.IsNullOrEmpty(appDto.NomorAppointment))
                return BadRequest("NomorAppointment is required");

            if (string.IsNullOrEmpty(appDto.Status))
                return BadRequest("Status is required");

            var sqlParameters = new[]
            {
                new SqlParameter("@Status", SqlDbType.Char, 1)
                {
                    Value = appDto.Status ?? (object)DBNull.Value
                },
                new SqlParameter("@NomorAppointment", SqlDbType.Char, 25)
                {
                    Value = appDto.NomorAppointment
                },
            };
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_Appointment_EditStatusTindakan " +
                    "@NomorAppointment = @NomorAppointment, " +
                    "@Status           = @Status",
                    sqlParameters);


                return Ok(new { message = "Edit status tindakan berhasil" });
            }
            catch (SqlException ex)
            {
                // Log the error details
                System.Diagnostics.Debug.WriteLine($"SQL Error: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the appointment");
            }
        }
    }
}
