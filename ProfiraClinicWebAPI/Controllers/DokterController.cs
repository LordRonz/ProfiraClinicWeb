using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinicWebAPI.Model;
using ProfiraClinicWebAPI.Data;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<MKARY>>> GetItems()
        {
            return _context.MKARY.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MKARY>> GetItem(string id)
        {
            var item = await _context.MKARY.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class BodyListOr
        {
            public string param { get; set; } = "%";
            public string getParam { get => this.param.Equals("%") ? this.param : $"%{this.param}%"; }
        }

        [HttpPost]
        public List<MKARY> GetDivisiListOr([FromBody] BodyListOr body)
        {
            return _context.MKARY
                .Where(d => (EF.Functions.Like(d.KDKAR, body.getParam) ||
                             EF.Functions.Like(d.NMKAR, body.getParam) ||
                             EF.Functions.Like(d.ALMT1, body.getParam) ||
                             EF.Functions.Like(d.TELP1, body.getParam)))
                .OrderBy(d => d.KDKAR)
                .ToList();
        }
    }
}
