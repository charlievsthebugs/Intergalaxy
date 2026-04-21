using System.Linq.Expressions;
using Intergalaxy.Application.Persistence;
using Intergalaxy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Intergalaxy.Infrastructure.Persistence;

public class EFReadRepository<T> : IReadRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;
    public EFReadRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public  IQueryable<T> GetAll() => _dbContext.Set<T>();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbContext.Set<T>().Where(predicate).ToListAsync();

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await _dbContext.Set<T>().AnyAsync(predicate);

    public IQueryable<T> FindAsQueryable(Expression<Func<T, bool>> predicate)
        => _dbContext.Set<T>().Where(predicate);
}
