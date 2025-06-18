using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class GroupBarangController
    : BaseCrudController<GroupBarang>
    {
        public GroupBarangController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<GroupBarang> DbSet
            => _context.GroupBarang;

        protected override IQueryable<GroupBarang> ApplySearch(
            IQueryable<GroupBarang> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NamaGroupBarang, likeParam)
                || EF.Functions.Like(d.KodeGroupBarang, likeParam));

        protected override IOrderedQueryable<GroupBarang> ApplyOrder(
            IQueryable<GroupBarang> q)
            => q.OrderBy(d => d.KodeGroupBarang);
    }
}
