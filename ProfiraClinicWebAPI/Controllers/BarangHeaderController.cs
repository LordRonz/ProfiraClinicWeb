using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class BarangHeaderController
    : BaseCrudController<BarangHeader>
    {
        public BarangHeaderController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<BarangHeader> DbSet
            => _context.BarangHeader;

        protected override IQueryable<BarangHeader> ApplySearch(
            IQueryable<BarangHeader> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeBarang, likeParam)
            || EF.Functions.Like(d.KodeBarang, likeParam));

        protected override IOrderedQueryable<BarangHeader> ApplyOrder(
            IQueryable<BarangHeader> q)
            => q.OrderBy(d => d.KodeBarang);

        [NonAction]
        public override Task<ActionResult<IEnumerable<BarangHeader>>> GetItems(string last = null)
            => base.GetItems(last);

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var list = await _context.BarangHeaderList
                .FromSqlRaw("EXEC dbo.usp_PBarang_List")
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("GetListItem")]
        public async Task<IActionResult> GetListItem()
        {
            var list = await _context.BarangHeaderList
                .FromSqlRaw("EXEC dbo.usp_MBarang_List")
                .ToListAsync();

            return Ok(list);
        }
    }
}
