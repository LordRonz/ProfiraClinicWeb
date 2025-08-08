using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class CPPTController
     : BaseCrudController<CPPT>
    {
        public class AddCPPTDto
        {
            public string? KodeLokasi { get; set; }
            public DateTime? TanggalTransaksi { get; set; }
            public string? NomorAppointment { get; set; }
            public string? KodeCustomer { get; set; }
            public string? KodeKaryawan { get; set; }
            public string? SUBYEKTIF { get; set; }
            public string? OBYEKTIF { get; set; }
            public string? ASSESTMENT { get; set; }
            public string? PLANNING { get; set; }
            public string? INSTRUKSI { get; set; }
            public string? INPMD { get; set; }  // e.g. 'A' or other code
        }



        public class CPPTListDto()
        {
            public string? KodeCustomer { get; set; }
        }

        public CPPTController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<CPPT> DbSet
            => _context.CPPT;

        protected override IQueryable<CPPT> ApplySearch(
            IQueryable<CPPT> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam)
            || EF.Functions.Like(d.KodeLokasi, likeParam)
            );

        protected override IOrderedQueryable<CPPT> ApplyOrder(
            IQueryable<CPPT> q)
            => q.OrderBy(d => d.TanggalTransaksi);

        [HttpPost("AddCPPT")]
        public async Task<IActionResult> AddCPPT([FromBody] AddCPPTDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName)) return Unauthorized();

            var user = await _context.MUser.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null) return NotFound();

            var karyawan = await _context.MKaryawan.FirstOrDefaultAsync(k => k.USRID == user.UserID);
            if (appDto.KodeKaryawan == null && karyawan == null) return NotFound();

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? user.KodeLokasi ?? (object)DBNull.Value),
                new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
                new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
                new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
                new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
                new SqlParameter("@SUBYEKTIF", appDto.SUBYEKTIF ?? (object)DBNull.Value),
                new SqlParameter("@OBYEKTIF", appDto.OBYEKTIF ?? (object)DBNull.Value),
                new SqlParameter("@ASSESTMENT", appDto.ASSESTMENT ?? (object)DBNull.Value),
                new SqlParameter("@PLANNING", appDto.PLANNING ?? (object)DBNull.Value),
                new SqlParameter("@INSTRUKSI", appDto.INSTRUKSI ?? (object)DBNull.Value),
                new SqlParameter("@USRID", user.UserID ?? (object)DBNull.Value),
                new SqlParameter("@INPMD", appDto.INPMD ?? (object)DBNull.Value),
                new SqlParameter
                {
                    ParameterName = "@NOFAK",
                    SqlDbType = System.Data.SqlDbType.Char,
                    Size = 25,
                    Direction = System.Data.ParameterDirection.Output
                }
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_CPPT_Add " +
                "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, @KodeCustomer, @KodeKaryawan, " +
                "@SUBYEKTIF, @OBYEKTIF, @ASSESTMENT, @PLANNING, @INSTRUKSI, " +
                "@USRID, @INPMD, @NOFAK OUTPUT",
                sqlParameters
            );

            var nomorTransaksi = sqlParameters.Last().Value?.ToString()?.Trim();

            return Ok(new { nomorTransaksi });
        }

        [HttpPost("EditCPPT")]
        public async Task<IActionResult> EditCPPT([FromBody] EditCPPTDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName)) return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return NotFound();

            var karyawan = await _context.MKaryawan
                                .FirstOrDefaultAsync(k => k.USRID == user.UserID);

            if (dto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            var delParams = new[]
            {
        new SqlParameter("@NomorTransaksi", dto.NomorTransaksi ?? (object)DBNull.Value)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_CPPT_Del @NomorTransaksi",
                delParams
            );

            try
            {
                var sqlParams = new[]
                {
            new SqlParameter("@KodeLokasi", dto.KodeLokasi ?? user.KodeLokasi ?? (object)DBNull.Value),
            new SqlParameter("@TanggalTransaksi", dto.TanggalTransaksi ?? DateTime.Now),
            new SqlParameter("@NomorAppointment", dto.NomorAppointment ?? (object)DBNull.Value),
            new SqlParameter("@KodeCustomer", dto.KodeCustomer ?? (object)DBNull.Value),
            new SqlParameter("@KodeKaryawan", dto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
            new SqlParameter("@SUBYEKTIF", dto.SUBYEKTIF ?? (object)DBNull.Value),
            new SqlParameter("@OBYEKTIF", dto.OBYEKTIF ?? (object)DBNull.Value),
            new SqlParameter("@ASSESTMENT", dto.ASSESTMENT ?? (object)DBNull.Value),
            new SqlParameter("@PLANNING", dto.PLANNING ?? (object)DBNull.Value),
            new SqlParameter("@INSTRUKSI", dto.INSTRUKSI ?? (object)DBNull.Value),
            new SqlParameter("@USRID", user.UserID ?? (object)DBNull.Value),
            new SqlParameter("@INPMD", dto.INPMD ?? (object)DBNull.Value),
            new SqlParameter("@NomorTransaksi", dto.NomorTransaksi ?? (object)DBNull.Value),
        };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_CPPT_Edit " +
                    "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, @KodeCustomer, @KodeKaryawan, " +
                    "@SUBYEKTIF, @OBYEKTIF, @ASSESTMENT, @PLANNING, @INSTRUKSI, " +
                    "@USRID, @INPMD, @NomorTransaksi",
                    sqlParams
                );

                return Ok(new { message = "CPPT updated successfully." });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Failed to update CPPT",
                    data = new { error = ex.Message }
                });
            }
        }



        [HttpPost("GetListTrm")]
        public async Task<IActionResult> GetListTrm([FromBody] CPPTListDto diagDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // 2) look it up
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.UserName == userName);

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeCustomer",  diagDto.KodeCustomer ?? (object)DBNull.Value),
            };

            var list = await _context.TRMCPPT
                .FromSqlRaw("EXEC dbo.usp_TRM_CPPT_List @KodeCustomer", sqlParameters)
                .ToListAsync();

            var result = new Pagination<TRMCPPT>
            {
                TotalCount = 0,
                Page = 0,
                PageSize = 0,
                Items = list
            };

            return Ok(result);
        }

        [HttpPost("GetByNomorAppointment")]
        public async Task<IActionResult> GetByNomorAppointment([FromBody] GetByNomorAppointmentDto dto)
        {
            var nomorAppointment = dto.NomorAppointment;
            if (string.IsNullOrWhiteSpace(nomorAppointment))
                return BadRequest(new { message = "NomorAppointment is required." });

            var cppt = await _context.CPPT
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorAppointment == nomorAppointment);

            return Ok(cppt);
        }
    }
}
