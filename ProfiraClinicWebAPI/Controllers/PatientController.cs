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
    public class PatientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MCustomer>>> GetItems()
        {
            return _context.MCustomer.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MCustomer>> GetItem(int id)
        {
            var item = await _context.MCustomer.FindAsync(id);

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
        public List<MCustomer> GetDivisiListOr([FromBody] BodyListOr body)
        {
            return _context.MCustomer
                .Where(d => (EF.Functions.Like(d.KDCUS, body.getParam) ||
                             EF.Functions.Like(d.ALAMAT, body.getParam) || 
                             EF.Functions.Like(d.KDCUS, body.getParam) ||
                             EF.Functions.Like(d.NOMHP, body.getParam)))
                .OrderBy(d => d.KDCUS)
                .ToList();
        }
    }
}
