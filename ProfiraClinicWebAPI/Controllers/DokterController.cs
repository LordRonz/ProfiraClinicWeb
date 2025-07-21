using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class DokterController
    : BaseCrudController<Dokter>
    {
        public DokterController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<Dokter> DbSet
            => _context.Dokter;

        protected override IQueryable<Dokter> ApplySearch(
            IQueryable<Dokter> q,
            string likeParam)
            => q.Where(d => (EF.Functions.Like(d.KodeKaryawan, likeParam) ||
                             EF.Functions.Like(d.KodeJenisDokter, likeParam) ||
                             EF.Functions.Like(d.KodeLokasi, likeParam) ||
                             EF.Functions.Like(d.KodeJabatan, likeParam)));

        protected override IOrderedQueryable<Dokter> ApplyOrder(
            IQueryable<Dokter> q)
            => q.OrderBy(d => d.UPDDT);

        protected override IQueryable<Dokter> ApplyLastFilter(
        IQueryable<Dokter> q,
        DateTime lastDate)
        {

            return q.Where(p =>
                p.UPDDT > lastDate
            );
        }

        [NonAction]
        public override Task<ActionResult> GetItems(string last = null, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
            => base.GetItems(last);

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var list = await _context.DokterList
                .FromSqlRaw("EXEC dbo.usp_PDokter_List")
                .ToListAsync();

            var result = new Pagination<DokterListDto>
            {
                TotalCount = 0,
                Page = 0,
                PageSize = 0,
                Items = list
            };

            return Ok(list);
        }
    }
}
