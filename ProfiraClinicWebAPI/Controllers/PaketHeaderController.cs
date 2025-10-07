using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        public override Task<ActionResult> GetItems(string last = null, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20, [FromQuery(Name = "sort")] string[] sort = null)
            => base.GetItems(last);

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var list = await _context.PaketHeaderList
                .FromSqlRaw("EXEC dbo.usp_PPaket_List")
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<PaketHeader>> GetItemByCode(string code)
        {
            return await FindOne(c => c.KodePaket == code);
        }

        // POST: api/PaketHeader
        // Create a new paketHeader record by executing a stored procedure with error handling.
        [HttpPost("add")]
        public async Task<ActionResult<PaketHeader>> CreateCustomerRiwayatAsal([FromBody] PaketHeader newPaketHeader)
        {
            if (newPaketHeader == null)
            {
                return BadRequest("PaketHeader data is null.");
            }

            // Prepare SQL parameters. For nullable fields, pass DBNull.Value.
            var sqlParameters = new[]
            {
                new SqlParameter("@KodeJenis", newPaketHeader.KodeJenis ?? (object)DBNull.Value),
                // The stored procedure generates the customer code, so pass an empty string.
                new SqlParameter("@KodeGroupPaket", newPaketHeader.KodeGroupPaket ?? (object)DBNull.Value),
                new SqlParameter("@KodePaket", newPaketHeader.KodePaket ?? (object)DBNull.Value),
                new SqlParameter("@NamaPaket", newPaketHeader.NamaPaket ?? (object)DBNull.Value),
                new SqlParameter("@HARGA", newPaketHeader.HARGA),
                new SqlParameter("@DiscMember", newPaketHeader.DiscMember),
                new SqlParameter("@DiscNonMember", newPaketHeader.DiscNonMember),
                new SqlParameter("@MasaLaku", newPaketHeader.MasaLaku),
                new SqlParameter("@AKTIF", newPaketHeader.Aktif ?? (object)DBNull.Value),
                new SqlParameter("@USRID", newPaketHeader.USRID ?? (object)DBNull.Value)
            };

            try
            {
                // Call the stored procedure using ExecuteSqlRawAsync.
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPaketH_Add " +
                    "@KodeJenis, @KodeGroupPaket, @KodePaket, @NamaPaket, @HARGA, @DiscMember, " +
                    "@DiscNonMember, @MasaLaku, @AKTIF, @USRID",
                    sqlParameters);

                // Return the newly created paketHeader. Adjust properties as needed.
                return CreatedAtAction(nameof(GetItem), new { id = newPaketHeader.KodePaket }, newPaketHeader);
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


        // DELETE: api/Patient/{id}
        // Delete a paketHeader record.
        [HttpDelete("del/{code}")]
        public async override Task<IActionResult> Delete(string code)
        {
            var affected = await _context.PaketHeader
        .Where(p => p.KodePaket == code)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(p => p.Aktif, "0")
            .SetProperty(p => p.UPDDT, DateTime.Now));
            if (affected == 0) return NotFound();
            return NoContent();
        }
    }
}
