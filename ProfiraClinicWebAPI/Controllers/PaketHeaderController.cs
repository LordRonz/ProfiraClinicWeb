using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class PaketHeaderController
    : BaseCrudController<PaketHeader>
    {
        public PaketHeaderController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<PaketHeader> DbSet
            => _context.PaketHeader;

        protected override IQueryable<PaketHeader> ApplySearch(
            IQueryable<PaketHeader> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NamaPaket, likeParam)
                || EF.Functions.Like(d.KodeGroupPaket, likeParam)
            || EF.Functions.Like(d.KodePaket, likeParam));

        protected override IOrderedQueryable<PaketHeader> ApplyOrder(
            IQueryable<PaketHeader> q)
            => q.OrderBy(d => d.KodePaket);
    }
}
