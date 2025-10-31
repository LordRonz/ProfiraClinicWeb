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

                var paket = await DbSet.FirstOrDefaultAsync(c => c.KodePaket == newPaketHeader.KodePaket);

                // Return the newly created paketHeader. Adjust properties as needed.
                return CreatedAtAction(nameof(GetItem), new { paket.IDPaketHeader }, paket);
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
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("KodePaket is required.");

            var pKode = new SqlParameter("@KodePaket", System.Data.SqlDbType.Char, 10)
            {
                Value = code
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPaket_Del @KodePaket",
                    pKode
                );

                return NoContent();
            }
            catch (SqlException ex)
            {
                // SP raises an error like: "Paket tersebut tidak ada ..."
                if (ex.Message.Contains("tidak ada", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = ex.Message });

                return BadRequest(new
                {
                    message = "Failed to delete paket.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected server error while deleting paket.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] PaketHeader updatedPaketHeader)
        {
            if (updatedPaketHeader == null)
                return BadRequest("PaketHeader data is required.");

            if (string.IsNullOrWhiteSpace(updatedPaketHeader.KodePaket))
                return BadRequest("KodePaket is required.");

            // Define parameters for DELETE
            var deleteParam = new SqlParameter("@KodePaket", System.Data.SqlDbType.Char, 10)
            {
                Value = updatedPaketHeader.KodePaket
            };

            // Define parameters for ADD
            var addParams = new[]
            {
        new SqlParameter("@KodeJenis", updatedPaketHeader.KodeJenis ?? (object)DBNull.Value),
        new SqlParameter("@KodeGroupPaket", updatedPaketHeader.KodeGroupPaket ?? (object)DBNull.Value),
        new SqlParameter("@KodePaket", updatedPaketHeader.KodePaket ?? (object)DBNull.Value),
        new SqlParameter("@NamaPaket", updatedPaketHeader.NamaPaket ?? (object)DBNull.Value),
        new SqlParameter("@HARGA", updatedPaketHeader.HARGA),
        new SqlParameter("@DiscMember", updatedPaketHeader.DiscMember),
        new SqlParameter("@DiscNonMember", updatedPaketHeader.DiscNonMember),
        new SqlParameter("@MasaLaku", updatedPaketHeader.MasaLaku),
        new SqlParameter("@AKTIF", updatedPaketHeader.Aktif ?? (object)DBNull.Value),
        new SqlParameter("@USRID", updatedPaketHeader.USRID ?? (object)DBNull.Value)
    };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Step 1: Delete the old record
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPaket_Del @KodePaket",
                    deleteParam
                );

                // Step 2: Add the new record
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PPaketH_Add " +
                    "@KodeJenis, @KodeGroupPaket, @KodePaket, @NamaPaket, @HARGA, " +
                    "@DiscMember, @DiscNonMember, @MasaLaku, @AKTIF, @USRID",
                    addParams
                );

                await transaction.CommitAsync();

                var paket = await DbSet.FirstOrDefaultAsync(c => c.KodePaket == updatedPaketHeader.KodePaket);

                return Ok(new
                {
                    message = "Paket successfully updated.",
                    kodePaket = updatedPaketHeader.KodePaket,
                    paket.IDPaketHeader,
                });
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();

                // Handle not found / RAISERROR from SP
                if (ex.Message.Contains("tidak ada", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = ex.Message });

                return BadRequest(new
                {
                    message = "Failed to update paket.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new
                {
                    message = "Unexpected error while editing paket.",
                    error = ex.Message
                });
            }
        }


    }
}
