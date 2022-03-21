using Lexicon_LMS_G1.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;
namespace Lexicon_LMS_G1.Data.Repositores;
public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext db;
    //private readonly IMapper mapper;
    protected readonly DbSet<T> dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        db = context;
        dbSet = db.Set<T>();
    }


    public virtual async Task<IEnumerable<T>> GetAsync()
    {
        return await db.Set<T>().ToListAsync();
    }
    public virtual async Task<IEnumerable<T>> GetIncludeAsync<Q>(params Expression<Func<T, Q>>[] expressions)
    {
        IQueryable<T> res = null;

        foreach (var expr in expressions)
        {
            res = dbSet.Include(expr);
        }

        return res.ToList();
    }
    public T? GetById(params object?[]? keyValues)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        T? item = db.Set<T>().Find(keyValues);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        return item;
    }

    public async Task<T?> GetByIdWithIncludedAsync<Q>(Expression<Func<T, Q>> includeExpression, Expression<Func<T, bool>> idExpression)
    {
        return await dbSet.Include(includeExpression).FirstOrDefaultAsync(idExpression);
    }

    public IEnumerable<T> GetByPredicate(Expression<Func<T, bool>> predicate)
    {
        return db.Set<T>().AsQueryable().Where(predicate).ToList();
    }
    public bool Update(T newItem, params object?[]? keyValues)
    {
        T? item = db.Set<T>().Find(keyValues);
        if (item == null) return false;
        if (newItem == null) return false;
        object itemPKvalue = GetPKeyValue(item);
        object newItemPKvalue = GetPKeyValue(newItem);
        if (newItemPKvalue != itemPKvalue) return false;
        db.Update(newItem);
        return true;
    }
    public bool Add(T newItem)
    {
        if (newItem == null) return false;
        db.Add(newItem);
        return true;
    }
    public bool Delete(params object?[]? keyValues)
    {
        T? item = db.Set<T>().Find(keyValues);
        if (item == null) return false;
        db.Remove(item);
        return true;
    }

    public virtual async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }

    //public bool Patch<Q>(JsonPatchDocument<Q> patchDocument, params object?[]? keyValues) where Q : class
    //{
    //    if (patchDocument == null)
    //    {
    //        return false;
    //    }
    //    T? _t = GetById(keyValues);
    //    if (_t == null)
    //    {
    //        return false;
    //    }
    //    Q _q = mapper.Map<Q>(_t);
    //    patchDocument.ApplyTo(_q);
    //    mapper.Map(_q, _t);
    //    return true;
    //}
    protected bool ItemExists(params object?[]? keyValue)
    {
        return db.Set<T>().Find(keyValue) != null;
    }
    private object GetPKeyValue(T entity)
    {
        //IEntityType entityType = db.GetService<IDbContextServices>().Model.FindEntityType(typeof(T));
        //IEnumerable<IProperty> properties = entityType.GetProperties();
        string primaryKeyName = db.GetService<IDbContextServices>().Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.First().Name;
        //List<T> sortedSet = (db.OrderBy(e => e.GetType().GetProperty(primaryKeyName).GetValue(e, null))).ToList();
        return entity.GetType().GetProperty(primaryKeyName).GetValue(entity, null);
    }
}