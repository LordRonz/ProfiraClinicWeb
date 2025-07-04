using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class KaryawanController
    : BaseCrudController<Karyawan>
    {
        public KaryawanController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<Karyawan> DbSet
            => _context.MKaryawan;

        protected override IQueryable<Karyawan> ApplySearch(
            IQueryable<Karyawan> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.NamaKaryawan, likeParam));

        protected override IOrderedQueryable<Karyawan> ApplyOrder(
            IQueryable<Karyawan> q)
            => q.OrderBy(d => d.UPDDT);
    }
}
