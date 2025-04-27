using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGroup>>> GetItems()
        {
            return _context.MUserGroup.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroup>> GetItem(string id)
        {
            var item = await _context.MUserGroup.FindAsync(Int64.Parse(id));

            if (item == null)
                return NotFound();

            return item;
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<UserGroup>> GetItemByCode(string code)
        {
            var item = await _context.MUserGroup.FirstOrDefaultAsync(c => c.KodeUserGroup == code);

            if (item == null)
                return NotFound();

            return item;
        }

        public class UserGroupBodyListOr
        {
            public string Param { get; set; } = "%";
            public string GetParam { get => this.Param.Equals("%") ? this.Param : $"%{this.Param}%"; }
        }

        // POST: api/Patient/search
        // Returns a list of patients matching the search parameters.
        [HttpPost("search")]
        public List<UserGroup> GetUserGroupListOr([FromBody] UserGroupBodyListOr body)
        {
            return _context.MUserGroup
                .Where(d => (EF.Functions.Like(d.KodeUserGroup, body.GetParam) ||
                             EF.Functions.Like(d.NamaUserGroup, body.GetParam)))
                .OrderBy(d => d.UPDDT)
                .ToList();
        }

        // POST: api/Patient
        // Create a new patient record by executing a stored procedure with error handling.
        [HttpPost]
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
        [HttpPut("{kode}")]
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
        [HttpDelete("{id}")]
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

        // Helper method to verify if a patient exists.
        private bool PatientExists(long id)
        {
            return _context.MCustomer.Any(e => e.IDCustomer == id);
        }
    }
}
