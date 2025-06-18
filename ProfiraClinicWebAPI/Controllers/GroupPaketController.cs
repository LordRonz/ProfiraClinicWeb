using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class GroupPaketController
    : BaseCrudController<GroupPaket>
    {
        public GroupPaketController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<GroupPaket> DbSet
            => _context.GroupPaket;

        protected override IQueryable<GroupPaket> ApplySearch(
            IQueryable<GroupPaket> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NamaGroupPaket, likeParam)
                || EF.Functions.Like(d.KodeGroupPaket, likeParam));

        protected override IOrderedQueryable<GroupPaket> ApplyOrder(
            IQueryable<GroupPaket> q)
            => q.OrderBy(d => d.KodeGroupPaket);
    }
}
