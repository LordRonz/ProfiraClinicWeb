using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Services;
using System.Security.Claims;

namespace ProfiraClinicWebAPI.Controllers
{
    public class PenandaanGambarListDto()
    {
        public string? KodeCustomer { get; set; }
    }

    public class PenandaanGambarListItemDto
    {
        public string NomorTransaksi { get; set; }
        public string? TRCD { get; set; }
        public string? TRSC { get; set; }
        public string KodeLokasi { get; set; }
        public string TahunTransaksi { get; set; }
        public string BulanTransaksi { get; set; }
        public DateTime TanggalTransaksi { get; set; }
        public string? NomorAppointment { get; set; }
        public string? KodeCustomer { get; set; }
        public string? KodeKaryawan { get; set; }
        public string? KodePoli { get; set; }
        public string? Keterangan { get; set; }
        public string? KetLk { get; set; }
        public DateTime? UPDDT { get; set; }
        public string? USRID { get; set; }
        public List<PenandaanGambarListDetailDto> Detail { get; set; } = new();
    }

    public class PenandaanGambarListDetailDto
    {
        public int IDDetail { get; set; }
        public string KodeGambar { get; set; }
        public string IDGambar { get; set; }
        public string? KETLK { get; set; }
        public string? NamaCustomer { get; set; }
        public string? NamaKaryawan { get; set; }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class PenandaanGambarController : BaseCrudController<TRMPenandaanGambarHeader>
    {
        private readonly string _kodePoli;
        public PenandaanGambarController(AppDbContext ctx, IConfiguration configuration) : base(ctx)
        {
            _kodePoli = configuration["KodePoli"];
        }

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
        public async Task<IActionResult> AddPenandaanGambarHeader([FromBody] AddPenandaanGambarHeaderDto appDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound("User not found");

            var kdLok = User.FindFirstValue(JwtClaimTypes.KodeLokasi);

            var karyawan = await _context.MKaryawan
                                .FirstOrDefaultAsync(k => k.USRID == user.KodeUser);

            if (appDto.KodeKaryawan == null && karyawan == null)
                return NotFound("Karyawan not found");

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi", appDto.KodeLokasi ?? kdLok ?? user.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", appDto.TanggalTransaksi ?? DateTime.Now),
        new SqlParameter("@NomorAppointment", appDto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer", appDto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodeKaryawan", appDto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
        new SqlParameter("@KodePoli", _kodePoli ?? (object)DBNull.Value),
        new SqlParameter("@Keterangan", appDto.Keterangan ?? (object)DBNull.Value),
        new SqlParameter("@USRID", user.USRID ?? (object)DBNull.Value),
        new SqlParameter
        {
            ParameterName = "@NomorTransaksi",
            SqlDbType = System.Data.SqlDbType.Char,
            Size = 25,
            Direction = System.Data.ParameterDirection.Output
        }
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_PenandaanGambar_Header_Add " +
                "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, @KodeCustomer, " +
                "@KodeKaryawan, @KodePoli, @Keterangan, @USRID, @NomorTransaksi OUTPUT",
                sqlParameters
            );

            var nomorTransaksi = sqlParameters.Last().Value?.ToString()?.Trim();

            return Ok(new { nomorTransaksi });
        }


        [HttpPost("EditPenandaanGambarHeader")]
        public async Task<IActionResult> EditPenandaanGambarHeader([FromBody] EditPenandaanGambarHeaderDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound("User not found");

            if (dto == null || string.IsNullOrWhiteSpace(dto.NomorTransaksi))
                return BadRequest("NomorTransaksi is required");

            var kdLok = User.FindFirstValue(JwtClaimTypes.KodeLokasi);

            var karyawan = await _context.MKaryawan
                                .FirstOrDefaultAsync(k => k.USRID == user.KodeUser);

            if (dto.KodeKaryawan == null && karyawan == null)
                return NotFound("Karyawan not found");

            try
            {
                var delParam = new[]
                {
        new SqlParameter("@NomorTransaksi", dto.NomorTransaksi ?? (object)DBNull.Value)
    };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_PenandaanGambar_Del @NomorTransaksi",
                    delParam
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi",       kdLok ?? user.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@TanggalTransaksi", DateTime.Now),
        new SqlParameter("@NomorAppointment", dto.NomorAppointment ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer",     dto.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@KodeKaryawan",     dto.KodeKaryawan ?? karyawan?.KodeKaryawan ?? (object)DBNull.Value),
        new SqlParameter("@KodePoli",         _kodePoli ?? (object)DBNull.Value),
        new SqlParameter("@Keterangan",       dto.Keterangan ?? (object)DBNull.Value),
        new SqlParameter("@USRID",            user.USRID ?? (object)DBNull.Value),
        new SqlParameter("@NomorTransaksi",   dto.NomorTransaksi)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_PenandaanGambar_Header_Edit " +
                "@KodeLokasi, @TanggalTransaksi, @NomorAppointment, @KodeCustomer, " +
                "@KodeKaryawan, @KodePoli, @Keterangan, @USRID, @NomorTransaksi",
                sqlParameters
            );

            return Ok(new { message = "Header updated successfully", nomorTransaksi = dto.NomorTransaksi });
        }


        // ===============================
        // Penandaan Gambar Detail
        // ===============================

        [HttpPost("AddPenandaanGambarDetail")]
        public async Task<IActionResult> AddPenandaanGambarDetail([FromBody] AddPenandaanGambarDetailDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _context.MUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.USRID == userName);

            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrEmpty(dto.NomorTransaksi))
                return BadRequest("NomorTransaksi is required");

            var sqlParameters = new[]
            {
        new SqlParameter("@NomorTransaksi", dto.NomorTransaksi),
        new SqlParameter("@KodeGambar", dto.KodeGambar ?? (object)DBNull.Value),
        new SqlParameter("@IDGambar", dto.IDGambar ?? (object)DBNull.Value),
        new SqlParameter
        {
            ParameterName = "@IDDetail",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Output
        }
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_PenandaanGambar_Detail_Add " +
                "@NomorTransaksi, @KodeGambar, @IDGambar, @IDDetail OUTPUT",
                sqlParameters
            );

            var idDetail = (int?)sqlParameters.Last().Value;

            return Ok(new { idDetail });
        }


        [HttpPost("EditPenandaanGambarDetail")]
        public async Task<IActionResult> EditPenandaanGambarDetail([FromBody] TRMPenandaanGambarDetail dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.NomorTransaksi))
                return BadRequest("NomorTransaksi is required");

            if (dto.IDGambar == null || dto.KodeGambar == null)
                return BadRequest("KodeGambar and IDGambar are required");

            try
            {
                var delParam = new[]
                {
        new SqlParameter("@NomorTransaksi", dto.NomorTransaksi ?? (object)DBNull.Value)
    };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_PenandaanGambar_Del @NomorTransaksi",
                    delParam
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            var sqlParameters = new[]
            {
        new SqlParameter("@NomorTransaksi", dto.NomorTransaksi),
        new SqlParameter("@KodeGambar", dto.KodeGambar ?? (object)DBNull.Value),
        new SqlParameter("@IDGambar", dto.IDGambar ?? (object)DBNull.Value),
        new SqlParameter("@IDDetail", dto.IDDetail)
    };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_TRM_PenandaanGambar_Detail_Edit " +
                "@NomorTransaksi, @KodeGambar, @IDGambar, @IDDetail",
                sqlParameters
            );

            return Ok(new { message = "Detail updated successfully", data = dto });
        }


        [HttpPost("GetListTrm")]
        public async Task<IActionResult> GetListTrm([FromBody] PenandaanGambarListDto penandaanGambarDto)
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
                new SqlParameter("@KodeCustomer",  penandaanGambarDto.KodeCustomer ?? (object)DBNull.Value),
            };

            var list = await _context.TRMPenandaanGambar
                .FromSqlRaw("EXEC dbo.usp_TRM_PenandaanGambar_List @KodeCustomer", sqlParameters)
                .ToListAsync();

            var items = list
    .GroupBy(h => h.NomorTransaksi)
    .Select(g =>
    {
        var h = g.First();
        return new PenandaanGambarListItemDto
        {
            NomorTransaksi = h.NomorTransaksi,
            TRCD = h.TRCD,
            TRSC = h.TRSC,
            KodeLokasi = h.KodeLokasi,
            TahunTransaksi = h.TahunTransaksi,
            BulanTransaksi = h.BulanTransaksi,
            TanggalTransaksi = h.TanggalTransaksi,
            NomorAppointment = h.NomorAppointment,
            KodeCustomer = h.KodeCustomer,
            KodeKaryawan = h.KodeKaryawan,
            KodePoli = h.KodePoli,
            Keterangan = h.Keterangan,
            KetLk = h.KETLK,
            UPDDT = h.UPDDT,
            USRID = h.USRID,
            Detail = g
                .Where(d => d.IDDetail != null && d.KodeGambar != null)
                .Select(d => new PenandaanGambarListDetailDto
                {
                    IDDetail = d.IDDetail ?? 0,
                    KodeGambar = d.KodeGambar,
                    IDGambar = d.IDGambar,
                    KETLK = d.KETLK,
                    NamaCustomer = d.NamaCustomer,
                    NamaKaryawan = d.NamaKaryawan
                })
                .ToList()
        };
    })
    .ToList();

            var result = new Pagination<PenandaanGambarListItemDto>
            {
                TotalCount = items.Count,
                Page = 0,
                PageSize = items.Count,
                Items = items
            };

            return Ok(result);

        }

        [HttpPost("GetByNomorAppointment")]
        public async Task<IActionResult> GetByNomorAppointment([FromBody] GetByNomorAppointmentDto dto)
        {
            var nomorAppointment = dto.NomorAppointment;
            if (string.IsNullOrWhiteSpace(nomorAppointment))
                return BadRequest(new { message = "NomorAppointment is required." });

            var header = await _context.TRMPenandaanGambarHeader
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.NomorAppointment == nomorAppointment);

            if (header == null)
                return Ok(null);

            var detail = await _context.TRMPenandaanGambarDetail
                .AsNoTracking()
                .Where(d => d.NomorTransaksi == header.NomorTransaksi).ToListAsync();

            dynamic response = new System.Dynamic.ExpandoObject();

            foreach (var prop in header.GetType().GetProperties())
            {
                ((IDictionary<string, object>)response)[prop.Name] = prop.GetValue(header);
            }

            response.detail = detail;
            return Ok(response);
        }
    }
}
