using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{

    public class AppointmentController
    : BaseCrudController<Appointment>
    {

        public class AppointmentListDto()
        {
            public string KodeLokasi {   get; set; }
            public DateTime? TanggalAppointment { get; set; }
            public string KodeKaryawan { get; set; }
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
            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? (object)DBNull.Value),

        new SqlParameter("@TanggalAppointment", appDto.TanggalAppointment ?? (object)DBNull.Value),

        new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? (object)DBNull.Value),

    };

            var list = await _context.Appointment
                .FromSqlRaw("EXEC dbo.usp_TRM_Appointment_List @KodeLokasi, @TanggalAppointment, @KodeKaryawan", sqlParameters)
                .ToListAsync();

            return Ok(list);
        }
    }
}
