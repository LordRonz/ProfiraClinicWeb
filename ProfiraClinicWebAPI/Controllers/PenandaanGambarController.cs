using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PenandaanGambarController : BaseCrudController<TRMPenandaanGambarHeader>
    {
        public PenandaanGambarController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<TRMPenandaanGambarHeader> DbSet
            => _context.TRMPenandaanGambarHeader;

        protected override IQueryable<TRMPenandaanGambarHeader> ApplySearch(
            IQueryable<TRMPenandaanGambarHeader> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan!, likeParam)
                || EF.Functions.Like(d.KodeCustomer!, likeParam));

        protected override IOrderedQueryable<TRMPenandaanGambarHeader> ApplyOrder(
            IQueryable<TRMPenandaanGambarHeader> q)
            => q.OrderBy(d => d.UPDDT);

        // ===============================
        // Penandaan Gambar Header
        // ===============================

        [HttpPost("AddPenandaanGambarHeader")]
        public async Task<IActionResult> AddPenandaanGambarHeader([FromBody] TRMPenandaanGambarHeader header)
        {
            if (header == null) return BadRequest("Invalid data");

            header.UPDDT = DateTime.Now;

            _context.TRMPenandaanGambarHeader.Add(header);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Header added successfully", data = header });
        }

        [HttpPost("EditPenandaanGambarHeader")]
        public async Task<IActionResult> EditPenandaanGambarHeader([FromBody] TRMPenandaanGambarHeader header)
        {
            if (header == null || string.IsNullOrEmpty(header.NomorTransaksi))
                return BadRequest("NomorTransaksi is required");

            var existing = await _context.TRMPenandaanGambarHeader.FindAsync(header.NomorTransaksi);
            if (existing == null) return NotFound("Header not found");

            _context.Entry(existing).CurrentValues.SetValues(header);
            existing.UPDDT = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Header updated successfully", data = existing });
        }

        // ===============================
        // Penandaan Gambar Detail
        // ===============================

        [HttpPost("AddPenandaanGambarDetail")]
        public async Task<IActionResult> AddPenandaanGambarDetail([FromBody] TRMPenandaanGambarDetail detail)
        {
            if (detail == null) return BadRequest("Invalid data");

            _context.TRMPenandaanGambarDetail.Add(detail);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Detail added successfully", data = detail });
        }

        [HttpPost("EditPenandaanGambarDetail")]
        public async Task<IActionResult> EditPenandaanGambarDetail([FromBody] TRMPenandaanGambarDetail detail)
        {
            if (detail == null || string.IsNullOrEmpty(detail.NomorTransaksi))
                return BadRequest("NomorTransaksi is required");

            var existing = await _context.TRMPenandaanGambarDetail.FindAsync(detail.NomorTransaksi);
            if (existing == null) return NotFound("Detail not found");

            _context.Entry(existing).CurrentValues.SetValues(detail);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Detail updated successfully", data = existing });
        }
    }
}
