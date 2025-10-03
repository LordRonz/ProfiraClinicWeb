using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class TindakanController : BaseCrudController<PPerawatanH>
    {
        public TindakanController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<PPerawatanH> DbSet
            => _context.PPerawatanH;

        protected override IQueryable<PPerawatanH> ApplySearch(
            IQueryable<PPerawatanH> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeJenis, likeParam)
                || EF.Functions.Like(d.KodeGroupPerawatan, likeParam)
                || EF.Functions.Like(d.KodePerawatan, likeParam)
                || EF.Functions.Like(d.NamaPerawatan, likeParam));

        protected override IOrderedQueryable<PPerawatanH> ApplyOrder(
            IQueryable<PPerawatanH> q)
            => q.OrderBy(d => d.KodePerawatan);

        protected override IQueryable<PPerawatanH> ApplyLastFilter(
            IQueryable<PPerawatanH> q,
            DateTime lastDate)
        {
            return q.Where(p => p.UpdDt > lastDate);
        }

        // POST: api/Tindakan/add
        [HttpPost("add")]
        public async Task<ActionResult<PPerawatanH>> AddTindakan([FromBody] PPerawatanH newTindakan)
        {
            if (newTindakan == null)
            {
                return BadRequest("Tindakan data is null.");
            }

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeJenis", newTindakan.KodeJenis ?? (object)DBNull.Value),
                new SqlParameter("@KategoriPerawatan", newTindakan.KategoriPerawatan ?? (object)DBNull.Value),
                new SqlParameter("@KodeGroupPerawatan", newTindakan.KodeGroupPerawatan ?? (object)DBNull.Value),
                new SqlParameter("@KodePerawatan", newTindakan.KodePerawatan ?? (object)DBNull.Value),
                new SqlParameter("@NamaPerawatan", newTindakan.NamaPerawatan ?? (object)DBNull.Value),
                new SqlParameter("@HARGA", newTindakan.Harga),
                new SqlParameter("@DiscMember", newTindakan.DiscMember),
                new SqlParameter("@DiscNonMember", newTindakan.DiscNonMember),
                new SqlParameter("@POINT", newTindakan.Point),
                new SqlParameter("@AKTIF", newTindakan.Aktif ?? (object)DBNull.Value),
                new SqlParameter("@USRID", newTindakan.UsrId ?? (object)DBNull.Value)
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPerawatanH_Add " +
                    "@KodeJenis, @KategoriPerawatan, @KodeGroupPerawatan, @KodePerawatan, @NamaPerawatan, " +
                    "@HARGA, @DiscMember, @DiscNonMember, @POINT, @AKTIF, @USRID",
                    sqlParameters);

                // Return the newly created tindakan object
                return CreatedAtAction(nameof(GetItem), new { id = newTindakan.KodePerawatan }, newTindakan);
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("del/{code}")]
        public async override Task<IActionResult> Delete(string code)
        {
            var affected = await _context.PPerawatanH
        .Where(p => p.KodePerawatan == code)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(p => p.Aktif, "0")
            .SetProperty(p => p.UpdDt, DateTime.Now));
            if (affected == 0) return NotFound();
            return NoContent();
        }
    }
}
