using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class AppointmentController
    : BaseCrudController<GroupBarang>
    {
        public AppointmentController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<Appointment> DbSet
            => _context.Appointment;

        protected override IQueryable<Appointment> ApplySearch(
            IQueryable<Appointment> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NamaGroupBarang, likeParam)
                || EF.Functions.Like(d.KodeGroupBarang, likeParam));

        protected override IOrderedQueryable<Appointment> ApplyOrder(
            IQueryable<Appointment> q)
            => q.OrderBy(d => d.KodeGroupBarang);
    }
}
