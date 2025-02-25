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
        public async Task<ActionResult<IEnumerable<PPERH>>> GetItems()
        {
            return _context.PPERH.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PPERH>> GetItem(string id)
        {
            var item = await _context.PPERH.FindAsync(id);

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
        public List<PPERH> GetDivisiListOr([FromBody] TindakanBodyListOr body)
        {
            return _context.PPERH
                .Where(d => (EF.Functions.Like(d.KdJns, body.GetParam) ||
                             EF.Functions.Like(d.KdGrp, body.GetParam) ||
                             EF.Functions.Like(d.KdPer, body.GetParam) ||
                             EF.Functions.Like(d.NmPer, body.GetParam)))
                .OrderBy(d => d.KdPer)
                .ToList();
        }
    }
}
