using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class UserController
: BaseCrudController<User>
    {
        public UserController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<User> DbSet
            => _context.MUser;

        protected override IQueryable<User> ApplySearch(
            IQueryable<User> q,
            string likeParam)
            => q.Where(d => (EF.Functions.Like(d.KodeUserGroup, likeParam) ||
                             EF.Functions.Like(d.USRID, likeParam)));

        protected override IOrderedQueryable<User> ApplyOrder(
            IQueryable<User> q)
            => q.OrderBy(d => d.UPDDT);

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<User>> GetItemByCode(string code)
        {
            var item = await _context.MUser.FirstOrDefaultAsync(c => c.KodeUserGroup == code);

            if (item == null)
                return NotFound();

            return item;
        }

        // POST: api/Patient
        // Create a new patient record by executing a stored procedure with error handling.
        [HttpPost("add")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("User data is null.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

            // Prepare SQL parameters. For nullable fields, pass DBNull.Value.
            var sqlParameters = new[]
            {
        new SqlParameter("@UserID", newUser.KodeUser ?? (object)DBNull.Value),
        // The stored procedure generates the customer code, so pass an empty string.
        new SqlParameter("@USRID", newUser.USRID ?? (object)DBNull.Value),
        new SqlParameter("@Password", hashedPassword ?? (object)DBNull.Value),
        new SqlParameter("@KodeUserGroup", newUser.KodeUserGroup ?? (object)DBNull.Value),
         new SqlParameter("@UserInput", newUser.UserInput ?? (object)DBNull.Value),
         new SqlParameter("@KodeLokasi", newUser.KodeLokasi ?? (object)DBNull.Value)
    };

            try
            {
                // Call the stored procedure using ExecuteSqlRawAsync.
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MUser_Add " +
                    "@UserID, @UserName, @Password, @KodeUserGroup, @UserInput, @KodeLokasi",
                    sqlParameters);

                // Return the newly created patient. Adjust properties as needed.
                return CreatedAtAction(nameof(GetItem), new { id = newUser.KodeUser }, newUser);
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
        public async Task<IActionResult> UpdateUser(string kode, [FromBody] User updatedUser)
        {
            if (updatedUser == null)
            {
                return BadRequest("User data is null.");
            }

            // Ensure that the provided route parameter matches the patient record's key.
            if (kode != updatedUser.KodeUserGroup)
            {
                return BadRequest("KodeUser mismatch between route and body.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);

            var sqlParameters = new[]
            {
                new SqlParameter("@UserID", updatedUser.KodeUser ?? (object)DBNull.Value),
                // The stored procedure generates the customer code, so pass an empty string.
                new SqlParameter("@UserName", updatedUser.USRID ?? (object)DBNull.Value),
                new SqlParameter("@Password", hashedPassword ?? (object)DBNull.Value),
                new SqlParameter("@KodeUserGroup", updatedUser.KodeUserGroup ?? (object)DBNull.Value),
                new SqlParameter("@UserInput", updatedUser.UserInput ?? (object)DBNull.Value),
                new SqlParameter("@KodeLokasi", updatedUser.KodeLokasi ?? (object)DBNull.Value)
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MUser_Edit " +
                    "@UserID, @UserName, @Password, @KodeUserGroup, @UserInput, @KodeLokasi",
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

        [HttpGet("me")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            // 1) grab the user ID from the JWT (must match whatever you set in your AuthService)
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // 2) look it up
            var user = await _context.MUser
                           .AsNoTracking()
                           .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound();

            // 3) clear out anything you don’t want to return
            user.Password = null;

            var karyawan = await _context.MKaryawan.FirstOrDefaultAsync(k => k.RefUserId == user.USRID);

            MKlinik? clinic = null;
            if (!string.IsNullOrEmpty(user.KodeLokasi))
            {
                clinic = await _context.MKlinik
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(k => k.KDLOK == user.KodeLokasi);
            }

            // 4) map to DTO (and clear password)
            var dto = new CurrentUser
            {
                UserID = user.KodeUser,
                UserName = user.USRID,
                KodeUserGroup = user.KodeUserGroup,
                KodeLokasi = user.KodeLokasi,
                Klinik = clinic,
                Karyawan = karyawan,
            };

            return Ok(dto);
        }


        // DELETE: api/Patient/{id}
        // Delete a patient record.
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var patient = await _context.MUser.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.MUser.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
