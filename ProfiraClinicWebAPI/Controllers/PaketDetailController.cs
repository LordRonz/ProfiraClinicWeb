using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class PaketDetailController
    : BaseCrudController<PaketDetail>
    {
        public PaketDetailController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<PaketDetail> DbSet
            => _context.PaketDetail;

        protected override IQueryable<PaketDetail> ApplySearch(
            IQueryable<PaketDetail> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodePaket, likeParam));

        protected override IOrderedQueryable<PaketDetail> ApplyOrder(
            IQueryable<PaketDetail> q)
            => q.OrderBy(d => d.KodePaket);

        [NonAction]
        public override Task<ActionResult> GetItems(string last = null, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20, [FromQuery(Name = "sort")] string[] sort = null)
            => base.GetItems(last);

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<PaketDetail>> GetItemByCode(string code)
        {
            return await FindOne(c => c.KodePaket == code);
        }

        // POST: api/PaketDetail
        // Create a new paketDetail record by executing a stored procedure with error handling.
        [HttpPost("add")]
        public async Task<ActionResult<PaketDetail>> CreateCustomerRiwayatAsal([FromBody] PaketDetail newPaketDetail)
        {
            if (newPaketDetail == null)
            {
                return BadRequest("PaketDetail data is null.");
            }

            // Prepare SQL parameters. For nullable fields, pass DBNull.Value.
            var sqlParameters = new[]
            {
                new SqlParameter("@IDPaketHeader", newPaketDetail.IDPaketHeader),
                // The stored procedure generates the customer code, so pass an empty string.
                new SqlParameter("@KodePaket", newPaketDetail.KodePaket ?? (object)DBNull.Value),
                new SqlParameter("@KodePerawatan", newPaketDetail.KodePerawatan ?? (object)DBNull.Value),
                new SqlParameter("@JumlahPerawatan", newPaketDetail.JumlahPerawatan),
            };

            try
            {
                // Call the stored procedure using ExecuteSqlRawAsync.
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPaketD_Add " +
                    "@IDPaketHeader, @KodePaket, @KodePerawatan, @JumlahPerawatan",
                    sqlParameters);

                // Return the newly created paketDetail. Adjust properties as needed.
                return CreatedAtAction(nameof(GetItem), new { newPaketDetail.IDPaketHeader }, newPaketDetail);
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // For other exceptions, return a 500 error.
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("GetByHeader/{idPaketHeader}")]
        public async Task<ActionResult<List<PaketDetail>>> GetItemByHeader(long idPaketHeader)
        {
            var items = await DbSet
        .Where(c => c.IDPaketHeader == idPaketHeader)
        .ToListAsync();
            return items;
        }


        // DELETE: api/Patient/{id}
        // Delete a paketDetail record.
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeletePaketDetail(long id)
        {
            var paketDetail = await _context.PaketDetail.FindAsync(id);
            if (paketDetail == null)
            {
                return NotFound();
            }

            _context.PaketDetail.Remove(paketDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
