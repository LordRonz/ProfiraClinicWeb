using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    public class GroupBarangController : BaseCrudController<GroupBarang>
    {
        public GroupBarangController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<GroupBarang> DbSet
            => _context.GroupBarang;

        protected override IQueryable<GroupBarang> ApplySearch(
            IQueryable<GroupBarang> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NamaGroupBarang, likeParam)
                || EF.Functions.Like(d.KodeGroupBarang, likeParam));

        protected override IOrderedQueryable<GroupBarang> ApplyOrder(
            IQueryable<GroupBarang> q)
            => q.OrderBy(d => d.KodeGroupBarang);

        public class AddGroupBarangDto
        {
            public string? KodeGroupBarang { get; set; }
            public string? NamaGroupBarang { get; set; }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddGroupBarang([FromBody] AddGroupBarangDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrWhiteSpace(dto.KodeGroupBarang))
                return BadRequest("KodeGroupBarang is required");
            if (string.IsNullOrWhiteSpace(dto.NamaGroupBarang))
                return BadRequest("NamaGroupBarang is required");

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeGroupBarang", dto.KodeGroupBarang),
                new SqlParameter("@NamaGroupBarang", dto.NamaGroupBarang),
                new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MGroupBarang_Add @KodeGroupBarang, @NamaGroupBarang, @USRID",
                    sqlParameters
                );

                return Ok(new
                {
                    message = "Group barang added successfully",
                    data = new { dto.KodeGroupBarang, dto.NamaGroupBarang }
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Failed to add group barang",
                    data = new { error = ex.Message }
                });
            }
        }
    }
}
