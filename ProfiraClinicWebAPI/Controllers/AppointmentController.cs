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
            public string KodeLokasi { get; set; }
            public DateTime? TanggalAppointment { get; set; }
            public string NomorAppointment { get; set; }
            public string KodeCustomer { get; set; }
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
                            .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return NotFound();


            var karyawan = await _context.MKaryawan.FirstOrDefaultAsync(k => k.USRID == user.UserID);

            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeLokasi",  appDto.KodeLokasi ?? user.KodeLokasi ?? (object)DBNull.Value),

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

            // 2) look it up
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return NotFound();

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? user.KodeLokasi ??(object) DBNull.Value),

                new SqlParameter("@TanggalAppointment", appDto.TanggalAppointment ?? (object)DBNull.Value),

                new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),

                new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
            };

            await _context.Database
                .ExecuteSqlRawAsync("EXEC dbo.usp_TRM_Appointment_EditStatusTindakan @KodeLokasi, @TanggalAppointment, @KodeCustomer, @NomorAppointment", sqlParameters);

            return Ok();
        }
    }
}
