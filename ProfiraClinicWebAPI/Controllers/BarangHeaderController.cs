using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    public class BarangHeaderController
    : BaseCrudController<BarangHeader>
    {
        public BarangHeaderController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<BarangHeader> DbSet
            => _context.BarangHeader;

        protected override IQueryable<BarangHeader> ApplySearch(
            IQueryable<BarangHeader> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeBarang, likeParam)
            || EF.Functions.Like(d.KodeBarang, likeParam));

        protected override IOrderedQueryable<BarangHeader> ApplyOrder(
            IQueryable<BarangHeader> q)
            => q.OrderBy(d => d.KodeBarang);

        [NonAction]
        public override Task<ActionResult> GetItems(string last = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
            => base.GetItems(last);

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var list = await _context.BarangHeaderList
                .FromSqlRaw("EXEC dbo.usp_PBarang_List")
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("GetListItem")]
        public async Task<IActionResult> GetListItem()
        {
            var list = await _context.BarangListDto
                .FromSqlRaw("EXEC dbo.usp_MBarang_List")
                .ToListAsync();

            return Ok(list);
        }



        [HttpPost("CreateBarangItem")]
        public async Task<IActionResult> CreateBarangItem([FromBody] CreateBarangItemDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid payload.");

            if (string.IsNullOrWhiteSpace(dto.KodeBarang))
                return BadRequest("KodeBarang is required.");
            if (string.IsNullOrWhiteSpace(dto.KodeGroupBarang))
                return BadRequest("KodeGroupBarang is required.");
            if (string.IsNullOrWhiteSpace(dto.NamaBarang))
                return BadRequest("NamaBarang is required.");
            if (string.IsNullOrWhiteSpace(dto.UnitDasar))
                return BadRequest("UnitDasar is required.");

            var parameters = new[]
            {
                new SqlParameter("@KodeBarang", dto.KodeBarang),
                new SqlParameter("@KodeGroupBarang", dto.KodeGroupBarang),
                new SqlParameter("@NamaBarang", dto.NamaBarang ?? (object)DBNull.Value),
                new SqlParameter("@KeteranganBarang", dto.KeteranganBarang ?? (object)DBNull.Value),
                new SqlParameter("@Unit1", dto.Unit1 ?? (object)DBNull.Value),
                new SqlParameter("@Unit2", dto.Unit2 ?? (object)DBNull.Value),
                new SqlParameter("@UnitDasar", dto.UnitDasar),
                new SqlParameter("@KonversiUnit1", dto.KonversiUnit1),
                new SqlParameter("@KonversiUnit2", dto.KonversiUnit2),
                new SqlParameter("@Aktif", dto.Aktif ?? "1"),
                new SqlParameter("@USRID", dto.USRID ?? (object)DBNull.Value),
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(@"
INSERT INTO MBarang
    (KodeBarang, KodeGroupBarang, NamaBarang, KeteranganBarang, Unit1, Unit2, UnitDasar,
     KonversiUnit1, KonversiUnit2, Aktif, UPDDT, USRID)
VALUES
    (@KodeBarang, @KodeGroupBarang, @NamaBarang, @KeteranganBarang, @Unit1, @Unit2, @UnitDasar,
     @KonversiUnit1, @KonversiUnit2, @Aktif, GETDATE(), @USRID);
", parameters);

                return Ok(new
                {
                    message = "Barang item created successfully.",
                    data = dto
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Failed to create barang item.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditBarang([FromBody] EditBarangHeaderDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid payload.");

            if (string.IsNullOrWhiteSpace(dto.KodeBarang))
                return BadRequest("KodeBarang is required.");

            if (string.IsNullOrWhiteSpace(dto.UnitJual))
                return BadRequest("UnitJual is required.");

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound("User not found");

            // Defaults & normalization
            var aktif = string.IsNullOrWhiteSpace(dto.Aktif) ? "1" : dto.Aktif.Trim();
            var usrId = string.IsNullOrWhiteSpace(dto.USRID) ? (user.USRID ?? string.Empty) : dto.USRID;

            // Build parameters exactly as SP expects
            var parameters = new[]
            {
        new SqlParameter("@KodeBarang", dto.KodeBarang),
        new SqlParameter("@UnitJual", dto.UnitJual),
        new SqlParameter("@HargaJual", dto.HargaJual),
        new SqlParameter("@DiscMember", dto.DiscMember),
        new SqlParameter("@DiscNonMember", dto.DiscNonMember),
        new SqlParameter("@Aktif", aktif),
        new SqlParameter("@USRID", usrId)
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PBarang_Edit " +
                    "@KodeBarang, @UnitJual, @HargaJual, @DiscMember, @DiscNonMember, @Aktif, @USRID",
                    parameters);

                return Ok(new
                {
                    message = "Barang header edited successfully.",
                    data = new
                    {
                        dto.KodeBarang,
                        dto.UnitJual,
                        dto.HargaJual,
                        dto.DiscMember,
                        dto.DiscNonMember,
                        Aktif = aktif,
                        USRID = usrId
                    }
                });
            }
            catch (SqlException ex)
            {
                // SP uses RAISERROR for business rules; surface it cleanly
                return BadRequest(new
                {
                    message = "Failed to edit barang item.",
                    error = ex.Message
                });
            }
        }


        [HttpDelete("del/{code}")]
        public async override Task<IActionResult> Delete(string code)
        {
            var affected = await _context.BarangHeader
        .Where(p => p.KodeBarang == code)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(p => p.Aktif, "0")
            .SetProperty(p => p.UPDDT, DateTime.Now));
            if (affected == 0) return NotFound();
            return NoContent();
        }

        public class CreateBarangHeaderDto
        {
            public string? KodeBarang { get; set; }
            public string? UnitJual { get; set; }
            public decimal HargaJual { get; set; }
            public decimal DiscMember { get; set; }
            public decimal DiscNonMember { get; set; }
            public string? USRID { get; set; }
        }

        // POST: api/BarangHeader/AddHeader
        [HttpPost("CreateBarangHeader")]
        public async Task<IActionResult> CreateBarangHeader([FromBody] CreateBarangHeaderDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid payload.");

            if (string.IsNullOrWhiteSpace(dto.KodeBarang))
                return BadRequest("KodeBarang is required.");
            if (string.IsNullOrWhiteSpace(dto.UnitJual))
                return BadRequest("UnitJual is required.");

            // Use authenticated user if available
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound("User not found");

            var usrId = string.IsNullOrWhiteSpace(dto.USRID) ? (user.USRID ?? string.Empty) : dto.USRID!;

            var parameters = new[]
            {
                new SqlParameter("@KodeBarang", dto.KodeBarang!),
                new SqlParameter("@UnitJual", dto.UnitJual!),
                new SqlParameter("@HargaJual", dto.HargaJual),
                new SqlParameter("@DiscMember", dto.DiscMember),
                new SqlParameter("@DiscNonMember", dto.DiscNonMember),
                new SqlParameter("@USRID", usrId)
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_PBarang_Add @KodeBarang, @UnitJual, @HargaJual, @DiscMember, @DiscNonMember, @USRID",
                    parameters);

                return Ok(new
                {
                    message = "Barang header created successfully.",
                    data = new
                    {
                        dto.KodeBarang,
                        dto.UnitJual,
                        dto.HargaJual,
                        dto.DiscMember,
                        dto.DiscNonMember,
                        USRID = usrId
                    }
                });
            }
            catch (SqlException ex)
            {
                // SP uses RAISERROR for business-rule failures; surface it nicely
                return BadRequest(new
                {
                    message = "Failed to create barang header.",
                    error = ex.Message
                });
            }
        }


        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<BarangHeader>> GetItemByCode(string code)
        {
            return await FindOne(c => c.KodeBarang == code);
        }
    }

}
