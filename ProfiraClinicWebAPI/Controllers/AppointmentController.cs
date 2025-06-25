using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class AppointmentController
    : BaseCrudController<Appointment>
    {
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
            || EF.Functions.Like(d.NomorHP, likeParam));

        protected override IOrderedQueryable<Appointment> ApplyOrder(
            IQueryable<Appointment> q)
            => q.OrderBy(d => d.TanggalAppointment);
    }
}
