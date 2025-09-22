using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    public class GroupPerawatanController : BaseCrudController<GroupPerawatan>
    {
        public GroupPerawatanController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<GroupPerawatan> DbSet
            => _context.GroupPerawatan;

        protected override IQueryable<GroupPerawatan> ApplySearch(
            IQueryable<GroupPerawatan> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NamaGroupPerawatan, likeParam)
                || EF.Functions.Like(d.KodeGroupPerawatan, likeParam));

        protected override IOrderedQueryable<GroupPerawatan> ApplyOrder(
            IQueryable<GroupPerawatan> q)
            => q.OrderBy(d => d.KodeGroupPerawatan);

        public class AddGroupPerawatanDto
        {
            public string? KodeGroupPerawatan { get; set; }
            public string? NamaGroupPerawatan { get; set; }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddGroupPerawatan([FromBody] AddGroupPerawatanDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrWhiteSpace(dto.KodeGroupPerawatan))
                return BadRequest("KodeGroupPerawatan is required");
            if (string.IsNullOrWhiteSpace(dto.NamaGroupPerawatan))
                return BadRequest("NamaGroupPerawatan is required");

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeGroupPerawatan", dto.KodeGroupPerawatan),
                new SqlParameter("@NamaGroupPerawatan", dto.NamaGroupPerawatan),
                new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MGroupPerawatan_Add @KodeGroupPerawatan, @NamaGroupPerawatan, @USRID",
                    sqlParameters
                );

                return Ok(new
                {
                    message = "Group perawatan added successfully",
                    data = new { dto.KodeGroupPerawatan, dto.NamaGroupPerawatan }
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Failed to add group perawatan",
                    data = new { error = ex.Message }
                });
            }
        }

        public class EditGroupPerawatanDto
        {
            public string? KodeGroupPerawatan { get; set; }
            public string? NamaGroupPerawatan { get; set; }
            public string? Aktif { get; set; } // '1' or '0'
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditGroupPerawatan([FromBody] EditGroupPerawatanDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrWhiteSpace(dto.KodeGroupPerawatan))
                return BadRequest("KodeGroupPerawatan is required");
            if (string.IsNullOrWhiteSpace(dto.NamaGroupPerawatan))
                return BadRequest("NamaGroupPerawatan is required");
            if (string.IsNullOrWhiteSpace(dto.Aktif))
                return BadRequest("Aktif is required");

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeGroupPerawatan", dto.KodeGroupPerawatan),
        new SqlParameter("@NamaGroupPerawatan", dto.NamaGroupPerawatan),
        new SqlParameter("@Aktif", dto.Aktif),
        new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MGroupPerawatan_Edit @KodeGroupPerawatan, @NamaGroupPerawatan, @Aktif, @USRID",
                    sqlParameters
                );

                return Ok(new
                {
                    message = "Group perawatan updated successfully",
                    data = new { dto.KodeGroupPerawatan, dto.NamaGroupPerawatan, dto.Aktif }
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Failed to update group perawatan",
                    data = new { error = ex.Message }
                });
            }
        }

        protected override IQueryable<GroupPerawatan> ApplyDeleteFilter(
    IQueryable<GroupPerawatan> q,
    string filter)
        {
            return q.Where(x => x.KodeGroupPerawatan == filter);
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest(new { statusCode = 400, message = "KodeGroupPerawatan is required." });

            var item = await _context.GroupPerawatan
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.KodeGroupPerawatan == code);

            if (item == null)
                return NotFound(new { statusCode = 404, message = $"Group paket '{code}' not found." });
            return Ok(item);
        }
    }
}
