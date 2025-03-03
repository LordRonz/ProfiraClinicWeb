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
    public class PatientController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MCustomer>>> GetItems()
        {
            return _context.MCustomer.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MCustomer>> GetItem(string id)
        {
            var item = await _context.MCustomer.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class PatientBodyListOr
        {
            public string Param { get; set; } = "%";
            public string GetParam { get => this.Param.Equals("%") ? this.Param : $"%{this.Param}%"; }
        }

        [HttpPost]
        public List<MCustomer> GetCustomerListOr([FromBody] PatientBodyListOr body)
        {
            return _context.MCustomer
                .Where(d => (EF.Functions.Like(d.KodeCustomer, body.GetParam) ||
                             EF.Functions.Like(d.ALAMAT, body.GetParam) ||
                             EF.Functions.Like(d.KodeCustomer, body.GetParam) ||
                             EF.Functions.Like(d.NomorHP, body.GetParam)))
                .OrderBy(d => d.KodeCustomer)
                .ToList();
        }
    }
}
