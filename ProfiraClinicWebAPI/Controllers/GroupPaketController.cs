using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    public class GroupPaketController
    : BaseCrudController<GroupPaket>
    {
        public GroupPaketController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<GroupPaket> DbSet
            => _context.GroupPaket;

        protected override IQueryable<GroupPaket> ApplySearch(
            IQueryable<GroupPaket> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NamaGroupPaket, likeParam)
                || EF.Functions.Like(d.KodeGroupPaket, likeParam));

        protected override IOrderedQueryable<GroupPaket> ApplyOrder(
            IQueryable<GroupPaket> q)
            => q.OrderBy(d => d.KodeGroupPaket);

        public class AddGroupPaketDto
        {
            public string? KodeGroupPaket { get; set; }
            public string? NamaGroupPaket { get; set; }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddGroupPaket([FromBody] AddGroupPaketDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // Get user (match your usual pattern: by Ref_USRID)
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound("User not found");

            // Validate required fields based on SP contract
            if (string.IsNullOrWhiteSpace(dto.KodeGroupPaket))
                return BadRequest("KodeGroupPaket is required");
            if (string.IsNullOrWhiteSpace(dto.NamaGroupPaket))
                return BadRequest("NamaGroupPaket is required");

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeGroupPaket", dto.KodeGroupPaket),
        new SqlParameter("@NamaGroupPaket", dto.NamaGroupPaket),
        new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MGroupPaket_Add @KodeGroupPaket, @NamaGroupPaket, @USRID",
                    sqlParameters
                );

                return Ok(new
                {
                    message = "Group paket added successfully",
                    data = new { dto.KodeGroupPaket, dto.NamaGroupPaket }
                });
            }
            catch (SqlException ex)
            {
                // Surface business-rule failures raised by RAISERROR from the SP
                return BadRequest(new
                {
                    message = "Failed to add group paket",
                    data = new { error = ex.Message }
                });
            }
        }
        public class EditGroupPaketDto
        {
            public string? KodeGroupPaket { get; set; }
            public string? NamaGroupPaket { get; set; }
            public string? Aktif { get; set; } // '1' or '0'
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditGroupPaket([FromBody] EditGroupPaketDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrWhiteSpace(dto.KodeGroupPaket))
                return BadRequest("KodeGroupPaket is required");
            if (string.IsNullOrWhiteSpace(dto.NamaGroupPaket))
                return BadRequest("NamaGroupPaket is required");
            if (string.IsNullOrWhiteSpace(dto.Aktif))
                return BadRequest("Aktif is required");

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeGroupPaket", dto.KodeGroupPaket),
        new SqlParameter("@NamaGroupPaket", dto.NamaGroupPaket),
        new SqlParameter("@Aktif", dto.Aktif),
        new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MGroupPaket_Edit @KodeGroupPaket, @NamaGroupPaket, @Aktif, @USRID",
                    sqlParameters
                );

                return Ok(new
                {
                    message = "Group paket updated successfully",
                    data = new { dto.KodeGroupPaket, dto.NamaGroupPaket, dto.Aktif }
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Failed to update group paket",
                    data = new { error = ex.Message }
                });
            }
        }

    }
}
