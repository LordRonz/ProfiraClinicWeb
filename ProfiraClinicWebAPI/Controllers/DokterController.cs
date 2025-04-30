using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DokterController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MKaryawan>>> GetItems()
        {
            return _context.MKaryawan.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MKaryawan>> GetItem(string id)
        {
            var item = await _context.MKaryawan.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class BodyListOr: BaseBodyListOr
        {
        }

        [HttpPost]
        public List<MKaryawan> GetDivisiListOr([FromBody] BodyListOr body)
        {
            return _context.MKaryawan
                .Where(d => (EF.Functions.Like(d.KodeKaryawan, body.GetParam) ||
                             EF.Functions.Like(d.NamaKaryawan, body.GetParam) ||
                             EF.Functions.Like(d.Alamat, body.GetParam) ||
                             EF.Functions.Like(d.NomorHP, body.GetParam)))
                .OrderBy(d => d.KodeKaryawan)
                .ToList();
        }
    }
}
