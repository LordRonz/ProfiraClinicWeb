using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class JabatanController
    : BaseCrudController<Jabatan>
    {
        public JabatanController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<Jabatan> DbSet
            => _context.Jabatan;

        protected override IQueryable<Jabatan> ApplySearch(
            IQueryable<Jabatan> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeJabatan, likeParam)
                || EF.Functions.Like(d.NamaJabatan, likeParam));

        protected override IOrderedQueryable<Jabatan> ApplyOrder(
            IQueryable<Jabatan> q)
            => q.OrderBy(d => d.UPDDT);
    }
}
