using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinicWebAPI.Model;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DokterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DokterController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MKARY>>> GetItems()
        {
            return _context.MKARY.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MKARY>> GetItem(int id)
        {
            var item = await _context.MKARY.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }
    }
}
