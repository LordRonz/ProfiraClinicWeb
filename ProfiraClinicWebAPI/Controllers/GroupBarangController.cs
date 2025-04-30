using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupBarangController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupBarang>>> GetItems()
        {
            return _context.GroupBarang.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupBarang>> GetItem(string id)
        {
            var item = await _context.GroupBarang.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class GroupBarangBodyListOr : BaseBodyListOr
        {
        }

        [HttpPost]
        public List<GroupBarang> GetGroupBarangListOr([FromBody] GroupBarangBodyListOr body)
        {
            return _context.GroupBarang
                .Where(d => (EF.Functions.Like(d.NamaGroupBarang, body.GetParam) || EF.Functions.Like(d.KodeGroupBarang, body.GetParam)))
                .OrderBy(d => d.KodeGroupBarang)
                .ToList();
        }
    }
}
