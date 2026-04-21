using System.Linq.Expressions;

namespace Intergalaxy.Application.Persistence;

public interface IReadRepository<T> where T : class
{
    IQueryable<T> GetAll();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    IQueryable<T> FindAsQueryable(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}
