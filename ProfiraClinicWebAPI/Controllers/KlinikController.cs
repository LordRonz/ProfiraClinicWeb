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
    public class KlinikController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MKlinik>>> GetItems()
        {
            return _context.MKlinik.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MKlinik>> GetItem(string id)
        {
            var item = await _context.MKlinik.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class KlinikBodyListOr
        {
            public string Param { get; set; } = "%";
            public string GetParam { get => this.Param.Equals("%") ? this.Param : $"%{this.Param}%"; }
        }

        [HttpPost]
        public List<MKlinik> GetCustomerListOr([FromBody] KlinikBodyListOr body)
        {
            return _context.MKlinik
                .Where(d => (EF.Functions.Like(d.KDLOK, body.GetParam) ||
                             EF.Functions.Like(d.ALAMAT, body.GetParam) ||
                             EF.Functions.Like(d.NAMAPT, body.GetParam) ||
                             EF.Functions.Like(d.ALAMATPT, body.GetParam) ||
                             EF.Functions.Like(d.TELP, body.GetParam) ||
                             EF.Functions.Like(d.KOTAPT, body.GetParam)))
                .OrderBy(d => d.KDLOK)
                .ToList();
        }
    }
}
