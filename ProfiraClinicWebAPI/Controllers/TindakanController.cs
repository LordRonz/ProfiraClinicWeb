using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinic.Models;
using ProfiraClinicWebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ProfiraClinicWebAPI.Controllers
{
    public class TindakanController(AppDbContext ctx)
        : BaseCrudController<PPerawatanH>(ctx)
    {
        protected override DbSet<PPerawatanH> DbSet
            => _context.PPerawatanH;

        protected override IQueryable<PPerawatanH> ApplySearch(
            IQueryable<PPerawatanH> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeJenis, likeParam)
                || EF.Functions.Like(d.KodeGroupPerawatan, likeParam)
            || EF.Functions.Like(d.KodePerawatan, likeParam)
            || EF.Functions.Like(d.NamaPerawatan, likeParam));

        protected override IOrderedQueryable<PPerawatanH> ApplyOrder(
            IQueryable<PPerawatanH> q)
            => q.OrderBy(d => d.KodePerawatan);

        protected override IQueryable<PPerawatanH> ApplyLastFilter(
        IQueryable<PPerawatanH> q,
        DateTime lastDate)
        {
            return q.Where(p =>
                p.UpdDt > lastDate
            );
        }
    }
}
