using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferensiController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Referensi>>> GetItems()
        {
            return _context.MRefferensi.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Referensi>> GetItem(string id)
        {
            var item = await _context.MRefferensi.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        public class ReferensiBodyListOr : BaseBodyListOr
        {
        }

        [HttpPost]
        public List<Referensi> GetCustomerListOr([FromBody] ReferensiBodyListOr body)
        {
            return _context.MRefferensi
                .Where(d => (EF.Functions.Like(d.Refferensi, body.GetParam)))
                .OrderBy(d => d.Refferensi)
                .ToList();
        }
    }
}
