using System.Linq.Expressions;

namespace Lexicon_LMS_G1.Data.Repositores
{
    public interface IBaseRepository<T> where T : class
    {
        bool Add(T newItem);
        bool Delete(params object?[]? keyValues);
        Task<IEnumerable<T>> GetAsync();
        Task<IEnumerable<T>> GetIncludeAsync<Q>(params Expression<Func<T, Q>>[] expression);
        T? GetById(params object?[]? keyValues);
        Task<T?> GetByIdWithIncludedAsync<Q>(Expression<Func<T, Q>> includeExpression, Expression<Func<T, bool>> idExpression);
        IEnumerable<T> GetByPredicate(Expression<Func<T, bool>> predicate);
        bool Update(T newItem, params object?[]? keyValues);
        Task SaveChangesAsync();
    }
}