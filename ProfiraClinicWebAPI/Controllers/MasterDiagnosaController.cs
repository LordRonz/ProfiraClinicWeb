using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class MasterDiagnosaController
    : BaseCrudController<MasterDiagnosa>
    {
        public MasterDiagnosaController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<MasterDiagnosa> DbSet
            => _context.MasterDiagnosa;

        protected override IQueryable<MasterDiagnosa> ApplySearch(
            IQueryable<MasterDiagnosa> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeDiagnosa, likeParam)
                || EF.Functions.Like(d.NamaDiagnosa, likeParam));

        protected override IOrderedQueryable<MasterDiagnosa> ApplyOrder(
            IQueryable<MasterDiagnosa> q)
            => q.OrderBy(d => d.UPDDT);
    }
}
