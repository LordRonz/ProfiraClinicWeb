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
    public class TindakanController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PPerawatanH>>> GetItems()
        {
            return _context.PPerawatanH.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PPerawatanH>> GetItem(string id)
        {
            var item = await _context.PPerawatanH.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class TindakanBodyListOr
        {
            public string Param { get; set; } = "%";
            public string GetParam { get => this.Param.Equals("%") ? this.Param : $"%{this.Param}%"; }
        }

        [HttpPost]
        public List<PPerawatanH> GetDivisiListOr([FromBody] TindakanBodyListOr body)
        {
            return _context.PPerawatanH
                .Where(d => (EF.Functions.Like(d.KodeJenis, body.GetParam) ||
                             EF.Functions.Like(d.KodeGroup, body.GetParam) ||
                             EF.Functions.Like(d.KodePerawatan, body.GetParam) ||
                             EF.Functions.Like(d.NamaPerawatan, body.GetParam)))
                .OrderBy(d => d.KodePerawatan)
                .ToList();
        }
    }
}
