using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
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
        public override Task<ActionResult> GetItems(string last = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
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
            var list = await _context.BarangListDto
                .FromSqlRaw("EXEC dbo.usp_MBarang_List")
                .ToListAsync();

            return Ok(list);
        }



        [HttpPost("CreateBarangItem")]
        public async Task<IActionResult> CreateBarangItem([FromBody] CreateBarangItemDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid payload.");

            if (string.IsNullOrWhiteSpace(dto.KodeBarang))
                return BadRequest("KodeBarang is required.");
            if (string.IsNullOrWhiteSpace(dto.KodeGroupBarang))
                return BadRequest("KodeGroupBarang is required.");
            if (string.IsNullOrWhiteSpace(dto.NamaBarang))
                return BadRequest("NamaBarang is required.");
            if (string.IsNullOrWhiteSpace(dto.UnitDasar))
                return BadRequest("UnitDasar is required.");

            var parameters = new[]
            {
                new SqlParameter("@KodeBarang", dto.KodeBarang),
                new SqlParameter("@KodeGroupBarang", dto.KodeGroupBarang),
                new SqlParameter("@NamaBarang", dto.NamaBarang ?? (object)DBNull.Value),
                new SqlParameter("@KeteranganBarang", dto.KeteranganBarang ?? (object)DBNull.Value),
                new SqlParameter("@Unit1", dto.Unit1 ?? (object)DBNull.Value),
                new SqlParameter("@Unit2", dto.Unit2 ?? (object)DBNull.Value),
                new SqlParameter("@UnitDasar", dto.UnitDasar),
                new SqlParameter("@KonversiUnit1", dto.KonversiUnit1),
                new SqlParameter("@KonversiUnit2", dto.KonversiUnit2),
                new SqlParameter("@Aktif", dto.Aktif ?? "1"),
                new SqlParameter("@USRID", dto.USRID ?? (object)DBNull.Value),
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(@"
INSERT INTO MBarang
    (KodeBarang, KodeGroupBarang, NamaBarang, KeteranganBarang, Unit1, Unit2, UnitDasar,
     KonversiUnit1, KonversiUnit2, Aktif, UPDDT, USRID)
VALUES
    (@KodeBarang, @KodeGroupBarang, @NamaBarang, @KeteranganBarang, @Unit1, @Unit2, @UnitDasar,
     @KonversiUnit1, @KonversiUnit2, @Aktif, GETDATE(), @USRID);
", parameters);

                return Ok(new
                {
                    message = "Barang item created successfully.",
                    data = dto
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Failed to create barang item.",
                    error = ex.Message
                });
            }
        }
    }
}
