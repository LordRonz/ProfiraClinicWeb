using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinicWebAPI.Model;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TindakanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TindakanController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PPERH>>> GetItems()
        {
            return _context.PPERH.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PPERH>> GetItem(int id)
        {
            var item = await _context.PPERH.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }
    }
}
