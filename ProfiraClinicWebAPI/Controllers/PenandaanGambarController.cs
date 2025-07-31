using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class PenandaanGambarController
    : BaseCrudController<TRMGambar>
    {
        public PenandaanGambarController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<TRMGambar> DbSet
            => _context.TRMGambar;

        protected override IQueryable<TRMGambar> ApplySearch(
            IQueryable<TRMGambar> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam));

        protected override IOrderedQueryable<TRMGambar> ApplyOrder(
            IQueryable<TRMGambar> q)
            => q.OrderBy(d => d.UPDDT);
    }
}
