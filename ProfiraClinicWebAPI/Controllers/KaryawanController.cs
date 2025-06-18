using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class KaryawanController
    : BaseCrudController<MKaryawan>
    {
        public KaryawanController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<MKaryawan> DbSet
            => _context.MKaryawan;

        protected override IQueryable<MKaryawan> ApplySearch(
            IQueryable<MKaryawan> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.NamaKaryawan, likeParam));

        protected override IOrderedQueryable<MKaryawan> ApplyOrder(
            IQueryable<MKaryawan> q)
            => q.OrderBy(d => d.UPDDT);
    }
}
