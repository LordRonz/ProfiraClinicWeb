using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class CustomerRiwayatAsalController
: BaseCrudController<CustomerRiwayatAsal>
    {
        public CustomerRiwayatAsalController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<CustomerRiwayatAsal> DbSet
            => _context.CustomerRiwayatAsal;

        protected override IQueryable<CustomerRiwayatAsal> ApplySearch(
            IQueryable<CustomerRiwayatAsal> q,
            string likeParam)
            => q.Where(d => (EF.Functions.Like(d.KodeCustomer, likeParam) ||
                             EF.Functions.Like(d.PenyakitDahulu, likeParam) ||
                             EF.Functions.Like(d.PenyakitSekarang, likeParam) ||
                             EF.Functions.Like(d.KetAlergiMakanan, likeParam)));

        protected override IOrderedQueryable<CustomerRiwayatAsal> ApplyOrder(
            IQueryable<CustomerRiwayatAsal> q)
            => q.OrderBy(d => d.KodeCustomer);

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<CustomerRiwayatAsal>> GetItemByCode(string code)
        {
            return await FindOne(c => c.KodeCustomer == code);
        }

        // POST: api/Patient
        // Create a new customerRiwayatAsal record by executing a stored procedure with error handling.
        [HttpPost("add")]
        public async Task<ActionResult<CustomerRiwayatAsal>> CreateCustomerRiwayatAsal([FromBody] CustomerRiwayatAsal newCustomerRiwayatAsal)
        {
            if (newCustomerRiwayatAsal == null)
            {
                return BadRequest("Patient data is null.");
            }

            // Prepare SQL parameters. For nullable fields, pass DBNull.Value.
            var sqlParameters = new[]
            {
                new SqlParameter("@KodeCustomer", newCustomerRiwayatAsal.KodeCustomer ?? (object)DBNull.Value),
                // The stored procedure generates the customer code, so pass an empty string.
                new SqlParameter("@PenyakitDahulu", newCustomerRiwayatAsal.PenyakitDahulu ?? (object)DBNull.Value),
                new SqlParameter("@chkPenyakit", newCustomerRiwayatAsal.chkPenyakit ?? (object)DBNull.Value),
                new SqlParameter("@PenyakitSekarang", newCustomerRiwayatAsal.PenyakitSekarang ?? (object)DBNull.Value),
                new SqlParameter("@chkAlergiObat", newCustomerRiwayatAsal.chkAlergiObat ?? (object)DBNull.Value),
                new SqlParameter("@KetAlergiObat", newCustomerRiwayatAsal.KetAlergiObat ?? (object)DBNull.Value),
                new SqlParameter("@chkAlergiMakanan", newCustomerRiwayatAsal.chkAlergiMakanan ?? (object)DBNull.Value),
                new SqlParameter("@KetAlergiMakanan", newCustomerRiwayatAsal.KetAlergiMakanan ?? (object)DBNull.Value),
                new SqlParameter("@chkResiko", newCustomerRiwayatAsal.chkResiko ?? (object)DBNull.Value),
                new SqlParameter("@KetResiko", newCustomerRiwayatAsal.KetResiko ?? (object)DBNull.Value)
            };

            try
            {
                // Call the stored procedure using ExecuteSqlRawAsync.
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MCustomer_Riwayat_Add " +
                    "@KodeCustomer, @PenyakitDahulu, @chkPenyakit, @PenyakitSekarang, @chkAlergiObat, @KetAlergiObat, " +
                    "@chkAlergiMakanan, @KetAlergiMakanan, @chkResiko, @KetResiko",
                    sqlParameters);

                // Return the newly created customerRiwayatAsal. Adjust properties as needed.
                return CreatedAtAction(nameof(GetItem), new { id = newCustomerRiwayatAsal.KodeCustomer }, newCustomerRiwayatAsal);
            }
            catch (SqlException ex)
            {
                // This will catch SQL exceptions, including the errors raised from RAISERROR in the stored procedure.
                // Return the error message provided by the stored procedure.
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // For other exceptions, return a 500 error.
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }


        // DELETE: api/Patient/{id}
        // Delete a customerRiwayatAsal record.
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeletePatient(long id)
        {
            var customerRiwayatAsal = await _context.CustomerRiwayatAsal.FindAsync(id);
            if (customerRiwayatAsal == null)
            {
                return NotFound();
            }

            _context.CustomerRiwayatAsal.Remove(customerRiwayatAsal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to verify if a customerRiwayatAsal exists.
        private bool PatientExists(long id)
        {
            return _context.MCustomer.Any(e => e.IDCustomer == id);
        }
    }
}
