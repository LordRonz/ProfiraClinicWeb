using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinic.Models;
using ProfiraClinicWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    public class KlinikController
    : BaseCrudController<MKlinik>
    {
        public KlinikController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<MKlinik> DbSet
            => _context.MKlinik;

        protected override IQueryable<MKlinik> ApplySearch(
            IQueryable<MKlinik> q,
            string likeParam)
            => q.Where(d => (EF.Functions.Like(d.KDLOK, likeParam) ||
                             EF.Functions.Like(d.ALAMAT, likeParam) ||
                             EF.Functions.Like(d.NAMAPT, likeParam) ||
                             EF.Functions.Like(d.ALAMATPT, likeParam) ||
                             EF.Functions.Like(d.TELP, likeParam) ||
                             EF.Functions.Like(d.KOTAPT, likeParam)));

        protected override IOrderedQueryable<MKlinik> ApplyOrder(
            IQueryable<MKlinik> q)
            => q.OrderBy(d => d.KDLOK);

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<MKlinik>> GetItemByCode(string code)
        {
            return await FindOne(c => c.KDLOK == code);
        }
    }
}
