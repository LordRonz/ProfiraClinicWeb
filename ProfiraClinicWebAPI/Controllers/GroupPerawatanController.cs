using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class GroupPerawatanController
    : BaseCrudController<GroupPerawatan>
    {
        public GroupPerawatanController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<GroupPerawatan> DbSet
            => _context.GroupPerawatan;

        protected override IQueryable<GroupPerawatan> ApplySearch(
            IQueryable<GroupPerawatan> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NamaGroupPerawatan, likeParam)
                || EF.Functions.Like(d.KodeGroupPerawatan, likeParam));

        protected override IOrderedQueryable<GroupPerawatan> ApplyOrder(
            IQueryable<GroupPerawatan> q)
            => q.OrderBy(d => d.KodeGroupPerawatan);
    }
}
