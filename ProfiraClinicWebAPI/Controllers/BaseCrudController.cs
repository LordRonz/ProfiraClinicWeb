using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfiraClinic.Models.Api;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

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

            var result = new Pagination<TEntity>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = items // List<MyDto>
            };

            return Ok(result);
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

        /// <summary>
        /// Optional override for controllers that need non-PK deletes (e.g., keyless entities).
        /// Example: return q.Where(e => EF.Property<string>(e, "KodeBarang") == filter);
        /// Default throws for keyless types unless overridden.
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyDeleteFilter(
            IQueryable<TEntity> query,
            string filter)
        {
            // Default: not implemented. Override in derived controllers that need this.
            throw new NotSupportedException(
                $"{typeof(TEntity).Name} does not support delete by filter. Override ApplyDeleteFilter in the controller.");
        }

        /// <summary>
        /// POST /api/{Entity}/Del
        /// - If entity has a single primary key and GetParam is provided, deletes by PK.
        /// - Otherwise calls ApplyDeleteFilter(query, GetParam) and deletes matching rows (max 50).
        /// </summary>
        [HttpDelete("Del/{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            var filter = id;
            if (string.IsNullOrEmpty(filter))
                return BadRequest(new { message = "`GetParam` is required for delete." });

            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var isKeyless = entityType?.FindPrimaryKey() == null;

            // Try PK path if not keyless and single-column PK exists
            if (!isKeyless && ApplyDeleteFilter == null)
            {
                var pk = entityType?.FindPrimaryKey();
                if (pk != null && pk.Properties.Count == 1)
                {
                    var keyProp = pk.Properties[0];
                    var keyClrType = keyProp.ClrType;

                    object keyValue;
                    try
                    {
                        // Convert string filter to the PK type
                        keyValue = Convert.ChangeType(filter, Nullable.GetUnderlyingType(keyClrType) ?? keyClrType, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        return BadRequest(new { message = $"Could not convert '{filter}' to key type {keyClrType.Name}." });
                    }

                    var found = await DbSet.FindAsync(keyValue);
                    if (found == null)
                        return NotFound(new { message = $"Entity '{typeof(TEntity).Name}' with key '{filter}' not found." });

                    DbSet.Remove(found);
                    try
                    {
                        await _context.SaveChangesAsync();
                        return Ok(new { message = $"Deleted '{typeof(TEntity).Name}' with key '{filter}'." });
                    }
                    catch (DbUpdateException ex)
                    {
                        return BadRequest(new
                        {
                            message = "Delete failed due to database constraints.",
                            error = ex.GetBaseException().Message
                        });
                    }
                }
            }

            // Fallback: filter-based delete (for keyless or non-PK deletes) via ApplyDeleteFilter
            IQueryable<TEntity> query;
            try
            {
                query = ApplyDeleteFilter(DbSet, filter);
            }
            catch (NotSupportedException nse)
            {
                return BadRequest(new { message = nse.Message });
            }

            var toDelete = await query.AsTracking().Take(51).ToListAsync();
            if (toDelete.Count == 0)
                return NotFound(new { message = $"Entity '{typeof(TEntity).Name}' with key '{filter}' not found." });
            if (toDelete.Count > 50)
                return BadRequest(new { message = "Refusing to delete more than 50 rows in one request." });

            DbSet.RemoveRange(toDelete);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Deleted {toDelete.Count} '{typeof(TEntity).Name}' row(s)." });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Delete failed due to database constraints.",
                    error = ex.GetBaseException().Message
                });
            }
        }

    }
}
