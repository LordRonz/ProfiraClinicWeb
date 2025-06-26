using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class BarangController
    : BaseCrudController<Barang>
    {
        public BarangController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<Barang> DbSet
            => _context.Barang;

        protected override IQueryable<Barang> ApplySearch(
            IQueryable<Barang> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeBarang, likeParam)
            || EF.Functions.Like(d.KodeBarang, likeParam));

        protected override IOrderedQueryable<Barang> ApplyOrder(
            IQueryable<Barang> q)
            => q.OrderBy(d => d.KodeBarang);

        [NonAction]
        public override Task<ActionResult> GetItems(string last = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20 )
            => base.GetItems(last);

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var list = await _context.Barang
                .FromSqlRaw("EXEC dbo.usp_PBarang_List")
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("GetListItem")]
        public async Task<IActionResult> GetListItem()
        {
            var list = await _context.Barang
                .FromSqlRaw("EXEC dbo.usp_MBarang_List")
                .ToListAsync();

            return Ok(list);
        }
    }
}
