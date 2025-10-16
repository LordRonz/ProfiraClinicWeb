using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class UserGroupOtorisasiController
: BaseCrudController<UserGroupOtorisasi>
    {
        public UserGroupOtorisasiController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<UserGroupOtorisasi> DbSet
            => _context.UserGroupOtorisasi;

        protected override IQueryable<UserGroupOtorisasi> ApplySearch(
            IQueryable<UserGroupOtorisasi> q,
            string likeParam)
            => q.Where(d => (EF.Functions.Like(d.KodeUserGroup, likeParam) ||
                             EF.Functions.Like(d.KodeAplikasi, likeParam)));

        protected override IOrderedQueryable<UserGroupOtorisasi> ApplyOrder(
            IQueryable<UserGroupOtorisasi> q)
            => q.OrderBy(d => d.UPDDT);

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<UserGroupOtorisasi>> GetItemByCode(string code)
        {
            var item = await _context.UserGroupOtorisasi.FirstOrDefaultAsync(c => c.KodeAplikasi == code);

            if (item == null)
                return NotFound();

            return item;
        }


        // DELETE: api/Patient/{id}
        // Delete a patient record.
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeletePatient(long id)
        {
            var ugo = await _context.UserGroupOtorisasi.FindAsync(id);
            if (ugo == null)
            {
                return NotFound();
            }

            _context.UserGroupOtorisasi.Remove(ugo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
