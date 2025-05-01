using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;
using System.Linq.Expressions;

namespace ProfiraClinicWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseCrudController<TEntity> : ControllerBase
    where TEntity : class
    {
        protected readonly AppDbContext _context;

        protected BaseCrudController(AppDbContext ctx)
        {
            _context = ctx;
        }

        /// <summary>
        /// The DbSet for this entity
        /// </summary>
        protected abstract DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// Apply your EF.Functions.Like search here
        /// </summary>
        protected abstract IQueryable<TEntity> ApplySearch(
            IQueryable<TEntity> query,
            string likeParam);

        /// <summary>
        /// Apply ordering after search
        /// </summary>
        protected abstract IOrderedQueryable<TEntity> ApplyOrder(
            IQueryable<TEntity> query);

        [HttpGet("GetList")]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetItems()
            => await DbSet.ToListAsync();

        [HttpGet("GetById/{id}")]
        public virtual async Task<ActionResult<TEntity>> GetItem(long id)
        {
            var item = await DbSet.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        [HttpPost("GetListByString")]
        public virtual async Task<ActionResult<List<TEntity>>> Search(
            [FromBody] BaseBodyListOr body)
        {
            var q = ApplySearch(DbSet, body.GetParam);
            q = ApplyOrder(q);
            return await q.ToListAsync();
        }


        /// <summary>
        /// Core “find one by arbitrary predicate” helper.
        /// </summary>
        protected async Task<ActionResult<TEntity>> FindOne(
            Expression<Func<TEntity, bool>> predicate)
        {
            var item = await DbSet.FirstOrDefaultAsync(predicate);
            return item == null ? NotFound() : item;
        }
    }
}
