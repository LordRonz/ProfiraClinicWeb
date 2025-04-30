using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPerawatanController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPerawatan>>> GetItems()
        {
            return _context.GroupPerawatan.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupPerawatan>> GetItem(string id)
        {
            var item = await _context.GroupPerawatan.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class GroupGroupPerawatanBodyListOr : BaseBodyListOr
        {
        }

        [HttpPost]
        public List<GroupPerawatan> GetCustomerListOr([FromBody] GroupGroupPerawatanBodyListOr body)
        {
            return _context.GroupPerawatan
                .Where(d => (EF.Functions.Like(d.NamaGroupPerawatan, body.GetParam) || EF.Functions.Like(d.KodeGroupPerawatan, body.GetParam)))
                .OrderBy(d => d.KodeGroupPerawatan)
                .ToList();
        }
    }
}
