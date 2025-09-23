using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class PerawatanDetailController : BaseCrudController<PerawatanDetail>
    {
        public PerawatanDetailController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<PerawatanDetail> DbSet
            => _context.PerawatanDetail;

        protected override IQueryable<PerawatanDetail> ApplySearch(
            IQueryable<PerawatanDetail> q,
            string likeParam)
            => q.Where(d => EF.Functions.Like(d.KodePerawatan, likeParam));

        protected override IOrderedQueryable<PerawatanDetail> ApplyOrder(
            IQueryable<PerawatanDetail> q)
            => q.OrderBy(d => d.KodePerawatan);

        [NonAction]
        public override Task<ActionResult> GetItems(string last = null, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
            => base.GetItems(last);

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<PerawatanDetail>> GetItemByCode(string code)
            => await FindOne(c => c.KodePerawatan == code);

        // POST: api/PerawatanDetail/add
        [HttpPost("add")]
        public async Task<ActionResult> AddPerawatanDetail([FromBody] AddPerawatanDetailDto dto)
        {
            if (dto == null) return BadRequest("Payload is null.");

            var sqlParameters = new[]
            {
                new SqlParameter("@IDPerawatanHeader", dto.IDPerawatanHeader),
                new SqlParameter("@KodePerawatan", dto.KodePerawatan ?? (object)DBNull.Value),
                new SqlParameter("@KodeBarang", dto.KodeBarang ?? (object)DBNull.Value),
                new SqlParameter("@UnitPakai", dto.UnitPakai ?? (object)DBNull.Value),
                new SqlParameter("@JumlahPakai", dto.JumlahPakai),
                new SqlParameter("@IndikatorPakai", dto.IndikatorPakai ?? (object)DBNull.Value),
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPerawatanD_Add " +
                    "@IDPerawatanHeader, @KodePerawatan, @KodeBarang, @UnitPakai, @JumlahPakai, @IndikatorPakai",
                    sqlParameters);

                // We don't get back an ID from the SP; return location by KodePerawatan.
                return CreatedAtAction(nameof(GetItemByCode), new { code = dto.KodePerawatan }, dto);
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Unexpected error.", details = ex.Message });
            }
        }

        // DELETE: api/PerawatanDetail/del/{id}
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeletePerawatanDetail(long id)
        {
            var paketDetail = await _context.PerawatanDetail.FindAsync(id);
            if (paketDetail == null) return NotFound();

            _context.PerawatanDetail.Remove(paketDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class AddPerawatanDetailDto
    {
        public long IDPerawatanHeader { get; set; }
        public string? KodePerawatan { get; set; }
        public string? KodeBarang { get; set; }
        public string? UnitPakai { get; set; }
        public decimal JumlahPakai { get; set; }
        public string? IndikatorPakai { get; set; } // e.g. 'Y'/'N' or '1'/'0'
    }
}
