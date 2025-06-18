using Microsoft.AspNetCore.Mvc;
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

        [NonAction]
        public override Task<ActionResult<IEnumerable<PaketHeader>>> GetItems(string last = null)
            => base.GetItems(last);

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var list = await _context.PaketHeaderList
                .FromSqlRaw("EXEC dbo.usp_PPaket_List")
                .ToListAsync();

            return Ok(list);
        }
    }
}
