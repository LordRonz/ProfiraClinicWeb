using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class JenisDokterController
    : BaseCrudController<JenisDokter>
    {
        public JenisDokterController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<JenisDokter> DbSet
            => _context.JenisDokter;

        protected override IQueryable<JenisDokter> ApplySearch(
            IQueryable<JenisDokter> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeJenisDokter, likeParam)
                || EF.Functions.Like(d.NamaJenisDokter, likeParam));

        protected override IOrderedQueryable<JenisDokter> ApplyOrder(
            IQueryable<JenisDokter> q)
            => q.OrderBy(d => d.UPDDT);
    }
}
