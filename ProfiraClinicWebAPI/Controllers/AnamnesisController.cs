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
    public class AnamnesisController
    : BaseCrudController<Anamnesis>
    {
        public class AnamnesisListDto()
        {
            public string? KodeCustomer { get; set; }
        }
        private readonly string _kodePoli;

        public AnamnesisController(AppDbContext ctx, IConfiguration configuration) : base(ctx)
        {
            _kodePoli = configuration["KodePoli"] ?? "";
        }

        protected override DbSet<Anamnesis> DbSet
            => _context.Anamnesis;

        protected override IQueryable<Anamnesis> ApplySearch(
            IQueryable<Anamnesis> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.KodeKaryawan, likeParam)
                || EF.Functions.Like(d.KodeCustomer, likeParam)
            || EF.Functions.Like(d.KodeLokasi, likeParam)
            );

        protected override IOrderedQueryable<Anamnesis> ApplyOrder(
            IQueryable<Anamnesis> q)
            => q.OrderBy(d => d.TanggalTransaksi);

        [HttpPost("AddAnamnesis")]
        public async Task<IActionResult> AddAnamnesis([FromBody] AddAnamnesisDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // Get User
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);
            if (user == null)
                return NotFound();

            // Get Karyawan
            var karyawan = await _context.MKaryawan.FirstOrDefaultAsync(k => k.USRID == user.KodeUser);
            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            // Prepare Parameters
            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? user.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
        new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
        new SqlParameter("@KodePoli", _kodePoli ?? (object)DBNull.Value),
        new SqlParameter("@KeteranganAnamnesis", appDto.KeteranganAnamnesis ?? (object)DBNull.Value),
        new SqlParameter("@USRID", user.KodeUser ?? (object)DBNull.Value),
        new SqlParameter
        {
            ParameterName = "@NomorTransaksi",
            SqlDbType = System.Data.SqlDbType.Char,
            Size = 25,
            Direction = System.Data.ParameterDirection.Output
        }
    };

            // Execute SP
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_Anamnesis_Add @KodeLokasi, @TanggalTransaksi, @NomorAppointment, " +
                "@KodeCustomer, @KodeKaryawan, @KodePoli, @KeteranganAnamnesis, @USRID, @NomorTransaksi OUTPUT",
                sqlParameters
            );

            var nomorTransaksi = sqlParameters.Last().Value?.ToString()?.Trim();

            return Ok(new { nomorTransaksi });
        }


        [HttpPost("EditAnamnesis")]
        public async Task<IActionResult> EditAnamnesis([FromBody] EditAnamnesisDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound();

            var karyawan = await _context.MKaryawan
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(k => k.USRID == user.KodeUser);

            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound();

            if (string.IsNullOrEmpty(appDto.NomorTransaksi))
                return BadRequest(new { message = "NomorTransaksi is required for editing." });

            // Step 1: Delete first
            var delParam = new[]
            {
        new SqlParameter("@NomorTransaksi", appDto.NomorTransaksi)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_Anamnesis_Del @NomorTransaksi",
                delParam
            );

            // Step 2: Insert again (Edit)
            var editParams = new[]
            {
        new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? user.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
        new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
        new SqlParameter("@KodePoli", _kodePoli ?? (object)DBNull.Value),
        new SqlParameter("@KeteranganAnamnesis", appDto.KeteranganAnamnesis ?? (object)DBNull.Value),
        new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
        new SqlParameter("@NomorTransaksi", appDto.NomorTransaksi)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_Anamnesis_Edit @KodeLokasi, @TanggalTransaksi, @NomorAppointment, " +
                "@KodeCustomer, @KodeKaryawan, @KodePoli, @KeteranganAnamnesis, @USRID, @NomorTransaksi",
                editParams
            );

            return Ok(new { nomorTransaksi = appDto.NomorTransaksi });
        }




        [HttpPost("GetListTrm")]
        public async Task<IActionResult> GetListTrm([FromBody] AnamnesisListDto diagDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            // 2) look it up
            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            var sqlParameters = new[]
            {
                new SqlParameter("@KodeCustomer",  diagDto.KodeCustomer ?? (object)DBNull.Value),
            };

            var list = await _context.TRMAnamnesis
                .FromSqlRaw("EXEC dbo.usp_TRM_Anamnesis_List @KodeCustomer", sqlParameters)
                .ToListAsync();

            var result = new Pagination<TRMAnamnesis>
            {
                TotalCount = 0,
                Page = 0,
                PageSize = 0,
                Items = list
            };

            return Ok(result);
        }

        [HttpPost("GetByNomorTransaksi")]
        public async Task<IActionResult> GetByNomorTransaksi([FromBody] GetByNomorTransaksiDto dto)
        {
            var nomorTransaksi = dto.NomorTransaksi;
            if (string.IsNullOrWhiteSpace(nomorTransaksi))
                return BadRequest(new { message = "NomorTransaksi is required." });

            var diagnosa = await _context.Anamnesis
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorTransaksi == nomorTransaksi);

            if (diagnosa == null)
                return Ok(new { message = "Anamnesis not found." });

            return Ok(diagnosa);
        }

        [HttpPost("GetByNomorAppointment")]
        public async Task<IActionResult> GetByNomorAppointment([FromBody] GetByNomorAppointmentDto dto)
        {
            var nomorAppointment = dto.NomorAppointment;
            if (string.IsNullOrWhiteSpace(nomorAppointment))
                return BadRequest(new { message = "NomorAppointment is required." });

            var diagnosa = await _context.Anamnesis
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorAppointment == nomorAppointment);

            return Ok(diagnosa);
        }
    }
}
