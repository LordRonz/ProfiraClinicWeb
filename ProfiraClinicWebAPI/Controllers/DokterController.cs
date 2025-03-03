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

        public class BodyListOr
        {
            public string param { get; set; } = "%";
            public string getParam { get => this.param.Equals("%") ? this.param : $"%{this.param}%"; }
        }

        [HttpPost]
        public List<MKaryawan> GetDivisiListOr([FromBody] BodyListOr body)
        {
            return _context.MKaryawan
                .Where(d => (EF.Functions.Like(d.KodeKaryawan, body.getParam) ||
                             EF.Functions.Like(d.NamaKaryawan, body.getParam) ||
                             EF.Functions.Like(d.Alamat1, body.getParam) ||
                             EF.Functions.Like(d.TELP1, body.getParam)))
                .OrderBy(d => d.KodeKaryawan)
                .ToList();
        }
    }
}
