using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        /// <summary>
        /// Override this to customize “changed‑since” filtering.
        /// Default impl looks for CreatedAt / UpdatedAt.
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyLastFilter(
            IQueryable<TEntity> query,
            DateTime lastDate)
        {
            // default: if your entities really have CreatedAt/UpdatedAt…
            var hasCreated = typeof(TEntity)
                .GetProperty("CreatedAt", BindingFlags.Public | BindingFlags.Instance) != null;
            var hasUpdated = typeof(TEntity)
                .GetProperty("UpdatedAt", BindingFlags.Public | BindingFlags.Instance) != null;

            if (hasCreated || hasUpdated)
            {
                query = query.Where(e =>
                    (hasCreated && EF.Property<DateTime>(e, "CreatedAt") > lastDate)
                 || (hasUpdated && EF.Property<DateTime>(e, "UpdatedAt") > lastDate)
                );
            }

            return query;
        }

        /// <summary>
        /// Get all items, or only those created/updated after `last` if provided
        /// </summary>
        // GET /api/YourEntity/GetList?last=20250502062345
        [HttpGet("GetList")]
        public virtual async Task<ActionResult> GetItems(
            [FromQuery] string last = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;
            if (pageSize > 100) pageSize = 100;

            var query = DbSet.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(last))
            {
                if (!DateTime.TryParseExact(
                        last,
                        "yyyyMMddHHmmss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var lastDate))
                {
                    return BadRequest($"`last` must be in yyyyMMddHHmmss format (you gave: '{last}')");
                }

                query = ApplyLastFilter(query, lastDate);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                items
            });
        }

        [HttpGet("GetById/{id}")]
        public virtual async Task<ActionResult<TEntity>> GetItem(long id)
        {
            var item = await DbSet.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        [HttpPost("GetListByString")]
        public virtual async Task<ActionResult> Search(
            [FromBody] BaseBodyListOr body,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;
            if (pageSize > 100) pageSize = 100;

            var query = ApplySearch(DbSet.AsNoTracking(), body.GetParam);
            System.Diagnostics.Debug.WriteLine(body.GetParam);
            query = ApplyOrder(query);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                items
            });
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
