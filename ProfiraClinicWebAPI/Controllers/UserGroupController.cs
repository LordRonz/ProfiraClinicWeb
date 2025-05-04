using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class UserGroupController
: BaseCrudController<UserGroup>
    {
        public UserGroupController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<UserGroup> DbSet
            => _context.MUserGroup;

        protected override IQueryable<UserGroup> ApplySearch(
            IQueryable<UserGroup> q,
            string likeParam)
            => q.Where(d => (EF.Functions.Like(d.KodeUserGroup, likeParam) ||
                             EF.Functions.Like(d.NamaUserGroup, likeParam)));

        protected override IOrderedQueryable<UserGroup> ApplyOrder(
            IQueryable<UserGroup> q)
            => q.OrderBy(d => d.UPDDT);

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<UserGroup>> GetItemByCode(string code)
        {
            var item = await _context.MUserGroup.FirstOrDefaultAsync(c => c.KodeUserGroup == code);

            if (item == null)
                return NotFound();

            return item;
        }

        // POST: api/Patient
        // Create a new patient record by executing a stored procedure with error handling.
        [HttpPost("add")]
        public async Task<ActionResult<UserGroup>> CreateUserGroup([FromBody] UserGroup newUserGroup)
        {
            if (newUserGroup == null)
            {
                return BadRequest("Patient data is null.");
            }

            // Prepare SQL parameters. For nullable fields, pass DBNull.Value.
            var sqlParameters = new[]
            {
        new SqlParameter("@KodeUserGroup", newUserGroup.KodeUserGroup ?? (object)DBNull.Value),
        new SqlParameter("@USRID", string.Empty),
        new SqlParameter("@NamaUserGroup", newUserGroup.NamaUserGroup ?? (object)DBNull.Value),
    };

            try
            {
                // Call the stored procedure using ExecuteSqlRawAsync.
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MUserGroup_Add " +
                    "@KodeUserGroup, @NamaUserGroup, @USRID",
                    sqlParameters);

                // Return the newly created patient. Adjust properties as needed.
                return CreatedAtAction(nameof(GetItem), new { id = newUserGroup.KodeUserGroup }, newUserGroup);
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


        // PUT: api/Patient/{kode}
        [HttpPut("edit/{kode}")]
        public async Task<IActionResult> UpdatePatient(string kode, [FromBody] UserGroup updatedUserGroup)
        {
            if (updatedUserGroup == null)
            {
                return BadRequest("Patient data is null.");
            }

            // Ensure that the provided route parameter matches the patient record's key.
            if (kode != updatedUserGroup.KodeUserGroup)
            {
                return BadRequest("KodeUserGroup mismatch between route and body.");
            }

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeUserGroup", updatedUserGroup.KodeUserGroup ?? (object)DBNull.Value),
        new SqlParameter("@USRID", string.Empty),
        new SqlParameter("@NamaUserGroup", updatedUserGroup.NamaUserGroup ?? (object)DBNull.Value),
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MUserGroup_Edit " +
                    "@KodeUserGroup, @NamaUserGroup, @USRID",
                    sqlParameters);

                return NoContent();
            }
            catch (SqlException ex)
            {
                // This catch block handles SQL exceptions including those raised by RAISERROR in the stored procedure.
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // General exception catch block.
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }


        // DELETE: api/Patient/{id}
        // Delete a patient record.
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeletePatient(long id)
        {
            var patient = await _context.MCustomer.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.MCustomer.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
