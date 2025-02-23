using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinicWebAPI.Model;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetItems()
        {
            return _context.Appointment.ToList();
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetItem(int id)
        {
            var item = await _context.Appointment.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }
    }
}
