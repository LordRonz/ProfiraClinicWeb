using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.ComponentModel.DataAnnotations;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class TRMPerawatanDetailController
        : BaseCrudController<TRMPerawatanDetail>
    {
        public TRMPerawatanDetailController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<TRMPerawatanDetail> DbSet
            => _context.TRMPerawatanDetail;

        protected override IQueryable<TRMPerawatanDetail> ApplySearch(
            IQueryable<TRMPerawatanDetail> q,
            string likeParam)
            => q.Where(d =>
                   EF.Functions.Like(d.NomorTransaksi!, likeParam)
                || EF.Functions.Like(d.KodePaket!, likeParam)
                || EF.Functions.Like(d.NomorFakturPaket!, likeParam)
                || EF.Functions.Like(d.KodePerawatan!, likeParam)
                || EF.Functions.Like(d.KodePerawatanPengganti!, likeParam)
                || EF.Functions.Like(d.KeteranganDetail!, likeParam)
                || EF.Functions.Like(d.KodeDokter!, likeParam)
                || EF.Functions.Like(d.KodePerawat1!, likeParam)
                || EF.Functions.Like(d.KodePerawat2!, likeParam)
            );

        protected override IOrderedQueryable<TRMPerawatanDetail> ApplyOrder(
            IQueryable<TRMPerawatanDetail> q)
            => q.OrderBy(d => d.NomorTransaksi).ThenBy(d => d.NomorUrut);

        // ----------------- DTOs -----------------
        public class AddTRMPerawatanDetailDto
        {
            [Required] public string NomorTransaksi { get; set; } = default!;   // FK to Header
            public int? NomorUrut { get; set; }                                  // if null, server can assign
            public string? JenisPerawatan { get; set; }                           // 'P' paket / 'R' rawat? (example)
            public string? KodePaket { get; set; }
            public string? NomorFakturPaket { get; set; }
            public string? KodePerawatan { get; set; }
            public string? KodePerawatanPengganti { get; set; }
            public decimal Qty { get; set; } = 1;
            public string? KeteranganDetail { get; set; }
            public string? KodeDokter { get; set; }
            public string? KodePerawat1 { get; set; }
            public string? KodePerawat2 { get; set; }
        }

        public class EditTRMPerawatanDetailDto : AddTRMPerawatanDetailDto
        {
            [Required] public new string NomorTransaksi { get; set; } = default!;
            [Required] public new int? NomorUrut { get; set; }                   // identify the row to edit
        }

        public class GetByNomorTransaksiDto
        {
            [Required] public string NomorTransaksi { get; set; } = default!;
        }

        public class GetDetailKeyDto
        {
            [Required] public string NomorTransaksi { get; set; } = default!;
            [Required] public int NomorUrut { get; set; }
        }

        // ----------------- CREATE -----------------
        [HttpPost("AddTRMPerawatanDetail")]
        public async Task<IActionResult> AddDetail([FromBody] AddTRMPerawatanDetailDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // SP expects INT
            var qtyInt = (int)Math.Round(dto.Qty);

            var p = new[]
            {
        new SqlParameter("@NomorTransaksi",            (object?)dto.NomorTransaksi ?? DBNull.Value),
        new SqlParameter("@JenisPerawatan",            (object?)dto.JenisPerawatan ?? DBNull.Value),
        new SqlParameter("@KodePaket",                 (object?)dto.KodePaket ?? DBNull.Value),
        new SqlParameter("@NomorFakturPaket",          (object?)dto.NomorFakturPaket ?? DBNull.Value),
        new SqlParameter("@KodePerawatan",             (object?)dto.KodePerawatan ?? DBNull.Value),
        new SqlParameter("@KodePerawatanPengganti",    (object?)dto.KodePerawatanPengganti ?? DBNull.Value),
        new SqlParameter("@QTY",                       qtyInt),
        new SqlParameter("@KodeDokter",                (object?)dto.KodeDokter ?? DBNull.Value),
        new SqlParameter("@KodePerawat1",              (object?)dto.KodePerawat1 ?? DBNull.Value),
        new SqlParameter("@KodePerawat2",              (object?)dto.KodePerawat2 ?? DBNull.Value),
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_Perawatan_Detail_Add " +
                    "@NomorTransaksi, @JenisPerawatan, @KodePaket, @NomorFakturPaket, " +
                    "@KodePerawatan, @KodePerawatanPengganti, @QTY, @KodeDokter, @KodePerawat1, @KodePerawat2",
                    p);

                // Note: SP doesn't return NomorUrut. If you need it, query back the last row or add output to SP later.
                return Ok(new { message = "Detail added.", data = dto });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { message = "Failed to add detail.", error = ex.Message });
            }
        }

        // ----------------- EDIT -----------------
        [HttpPost("EditTRMPerawatanDetail")]
        public async Task<IActionResult> EditDetail([FromBody] EditTRMPerawatanDetailDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (dto.NomorUrut is null) return BadRequest("NomorUrut is required for editing.");

            // 1) Remove existing row by composite key (EF delete; no SP provided for delete)
            var existing = await _context.TRMPerawatanDetail
                .FirstOrDefaultAsync(d => d.NomorTransaksi == dto.NomorTransaksi
                                       && d.NomorUrut == dto.NomorUrut.Value);

            if (existing != null)
            {
                _context.TRMPerawatanDetail.Remove(existing);
                await _context.SaveChangesAsync();
            }

            // 2) Re-insert using the EDIT SP (note: it inserts)
            var qtyInt = (int)Math.Round(dto.Qty);

            var p = new[]
            {
        new SqlParameter("@NomorTransaksi",            (object?)dto.NomorTransaksi ?? DBNull.Value),
        new SqlParameter("@JenisPerawatan",            (object?)dto.JenisPerawatan ?? DBNull.Value),
        new SqlParameter("@KodePaket",                 (object?)dto.KodePaket ?? DBNull.Value),
        new SqlParameter("@NomorFakturPaket",          (object?)dto.NomorFakturPaket ?? DBNull.Value),
        new SqlParameter("@KodePerawatan",             (object?)dto.KodePerawatan ?? DBNull.Value),
        new SqlParameter("@KodePerawatanPengganti",    (object?)dto.KodePerawatanPengganti ?? DBNull.Value),
        new SqlParameter("@QTY",                       qtyInt),
        new SqlParameter("@KodeDokter",                (object?)dto.KodeDokter ?? DBNull.Value),
        new SqlParameter("@KodePerawat1",              (object?)dto.KodePerawat1 ?? DBNull.Value),
        new SqlParameter("@KodePerawat2",              (object?)dto.KodePerawat2 ?? DBNull.Value),
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_TRM_Perawatan_Detail_Edit " +
                    "@NomorTransaksi, @JenisPerawatan, @KodePaket, @NomorFakturPaket, " +
                    "@KodePerawatan, @KodePerawatanPengganti, @QTY, @KodeDokter, @KodePerawat1, @KodePerawat2",
                    p);

                return Ok(new { message = "Detail updated (reinserted).", data = dto });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { message = "Failed to edit detail.", error = ex.Message });
            }
        }
    }
}
