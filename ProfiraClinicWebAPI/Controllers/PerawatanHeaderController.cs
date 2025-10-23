using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class PerawatanHeaderController : BaseCrudController<PerawatanHeader>
    {
        public PerawatanHeaderController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<PerawatanHeader> DbSet
            => _context.PPerawatanH;

        protected override IQueryable<PerawatanHeader> ApplySearch(
            IQueryable<PerawatanHeader> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeJenis, likeParam)
                || EF.Functions.Like(d.KodeGroupPerawatan, likeParam)
                || EF.Functions.Like(d.KodePerawatan, likeParam)
                || EF.Functions.Like(d.NamaPerawatan, likeParam));

        protected override IOrderedQueryable<PerawatanHeader> ApplyOrder(
            IQueryable<PerawatanHeader> q)
            => q.OrderBy(d => d.KodePerawatan);

        protected override IQueryable<PerawatanHeader> ApplyLastFilter(
            IQueryable<PerawatanHeader> q,
            DateTime lastDate)
        {
            return q.Where(p => p.UpdDt > lastDate);
        }


        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<PerawatanHeader>> GetItemByCode(string code)
        {
            return await FindOne(c => c.KodePerawatan == code);
        }

        // POST: api/Tindakan/add
        [HttpPost("add")]
        public async Task<ActionResult<PerawatanHeader>> AddTindakan([FromBody] PerawatanHeader newTindakan)
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
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("KodePerawatan is required.");

            var pKode = new SqlParameter("@KodePerawatan", System.Data.SqlDbType.Char, 10)
            {
                Value = code
            };

            try
            {
                // Call your SP that hard-deletes from PPerawatanH + PPerawatanD
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPerawatan_Del @KodePerawatan",
                    pKode
                );

                // SP completed successfully
                return NoContent();
            }
            catch (SqlException ex)
            {
                // SP uses RAISERROR when the code is not found — convert to 404 for API consumers
                if (ex.Message.Contains("tidak ada", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = ex.Message });

                // Other SQL errors -> 400 with details
                return BadRequest(new
                {
                    message = "Failed to delete perawatan.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Non-SQL exceptions
                return StatusCode(500, new
                {
                    message = "Unexpected server error while deleting perawatan.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] PerawatanHeader updated)
        {
            if (updated == null)
                return BadRequest("Payload is required.");
            if (string.IsNullOrWhiteSpace(updated.KodePerawatan))
                return BadRequest("KodePerawatan is required.");

            // Params for DELETE
            var delParam = new SqlParameter("@KodePerawatan", System.Data.SqlDbType.Char, 10)
            {
                Value = updated.KodePerawatan
            };

            // Params for ADD (same mapping as your add endpoint)
            var addParams = new[]
            {
        new SqlParameter("@KodeJenis",            updated.KodeJenis             ?? (object)DBNull.Value),
        new SqlParameter("@KategoriPerawatan",    updated.KategoriPerawatan     ?? (object)DBNull.Value),
        new SqlParameter("@KodeGroupPerawatan",   updated.KodeGroupPerawatan    ?? (object)DBNull.Value),
        new SqlParameter("@KodePerawatan",        updated.KodePerawatan         ?? (object)DBNull.Value),
        new SqlParameter("@NamaPerawatan",        updated.NamaPerawatan         ?? (object)DBNull.Value),
        new SqlParameter("@HARGA",                updated.Harga),
        new SqlParameter("@DiscMember",           updated.DiscMember),
        new SqlParameter("@DiscNonMember",        updated.DiscNonMember),
        new SqlParameter("@POINT",                updated.Point),
        new SqlParameter("@AKTIF",                updated.Aktif                 ?? (object)DBNull.Value),
        new SqlParameter("@USRID",                updated.UsrId                 ?? (object)DBNull.Value),
    };

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1) Delete existing
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPerawatan_Del @KodePerawatan",
                    delParam
                );

                // 2) Re-insert updated
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPerawatanH_Add " +
                    "@KodeJenis, @KategoriPerawatan, @KodeGroupPerawatan, @KodePerawatan, @NamaPerawatan, " +
                    "@HARGA, @DiscMember, @DiscNonMember, @POINT, @AKTIF, @USRID",
                    addParams
                );

                await tx.CommitAsync();

                return Ok(new
                {
                    message = "Perawatan successfully updated.",
                    kodePerawatan = updated.KodePerawatan
                });
            }
            catch (SqlException ex)
            {
                await tx.RollbackAsync();

                // SP RAISERROR: not found on delete, surface 404
                if (ex.Message.Contains("tidak ada", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = ex.Message });

                return BadRequest(new
                {
                    message = "Failed to update perawatan.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, new
                {
                    message = "Unexpected server error while editing perawatan.",
                    error = ex.Message
                });
            }
        }


    }
}
