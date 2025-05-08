using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using ProfiraClinicWebAPI.Helper;

namespace ProfiraClinicWebAPI.Controllers
{
    [Authorize]
    public class DokterController
    : BaseCrudController<Dokter>
    {
        public DokterController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<Dokter> DbSet
            => _context.Dokter;

        protected override IQueryable<Dokter> ApplySearch(
            IQueryable<Dokter> q,
            string likeParam)
            => q.Where(d => (EF.Functions.Like(d.KodeKaryawan, likeParam) ||
                             EF.Functions.Like(d.KodeJenisDokter, likeParam) ||
                             EF.Functions.Like(d.KodeLokasi, likeParam) ||
                             EF.Functions.Like(d.KodeJabatan, likeParam)));

        protected override IOrderedQueryable<Dokter> ApplyOrder(
            IQueryable<Dokter> q)
            => q.OrderBy(d => d.UPDDT);

        protected override IQueryable<Dokter> ApplyLastFilter(
        IQueryable<Dokter> q,
        DateTime lastDate)
        {

            return q.Where(p =>
                p.UPDDT > lastDate
            );
        }
    }
}
