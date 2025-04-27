using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPaketController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPaket>>> GetItems()
        {
            return _context.GroupPaket.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupPaket>> GetItem(string id)
        {
            var item = await _context.GroupPaket.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class GroupPaketBodyListOr
        {
            public string Param { get; set; } = "%";
            public string GetParam { get => this.Param.Equals("%") ? this.Param : $"%{this.Param}%"; }
        }

        [HttpPost]
        public List<GroupPaket> GetCustomerListOr([FromBody] GroupPaketBodyListOr body)
        {
            return _context.GroupPaket
                .Where(d => (EF.Functions.Like(d.NamaGroupPaket, body.GetParam)))
                .OrderBy(d => d.KodeGroupPaket)
                .ToList();
        }
    }
}
