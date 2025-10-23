using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using System.Data;

namespace ProfiraClinicWebAPI.Controllers
{
    public class SaldoPaketController
    : BaseCrudController<SaldoPaket>
    {
        public SaldoPaketController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<SaldoPaket> DbSet
            => _context.SaldoPaket;

        protected override IQueryable<SaldoPaket> ApplySearch(
            IQueryable<SaldoPaket> q,
            string likeParam)
            => q.Where(d
                => EF.Functions.Like(d.NMPKT, likeParam)
                || EF.Functions.Like(d.NMCUS, likeParam));

        protected override IOrderedQueryable<SaldoPaket> ApplyOrder(
            IQueryable<SaldoPaket> q)
            => q.OrderBy(d => d.TGAKH);

        /// <summary>
        /// Executes dbo.usp_VSaldoPaket and returns the saldo paket rows for a customer.
        /// </summary>
        [HttpGet("GetSaldoPaket/{kodeCustomer}")]
        public async Task<IActionResult> GetSaldoPaket(string kodeCustomer)
        {
            if (string.IsNullOrWhiteSpace(kodeCustomer))
                return BadRequest("KodeCustomer is required.");

            var pKode = new SqlParameter("@KodeCustomer", SqlDbType.Char, 10)
            {
                Value = kodeCustomer
            };

            try
            {
                var list = await _context.SaldoPaket
                    .FromSqlRaw("EXEC dbo.usp_VSaldoPaket @KodeCustomer", pKode)
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(list);
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Failed to load saldo paket.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected server error while loading saldo paket.",
                    error = ex.Message
                });
            }
        }
    }
}
