using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinicWebAPI.Model;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KlinikController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KlinikController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MKlinik>>> GetItems()
        {
            return _context.MKlinik.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MKlinik>> GetItem(int id)
        {
            var item = await _context.MKlinik.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }
    }
}
