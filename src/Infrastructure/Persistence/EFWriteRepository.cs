using Intergalaxy.Application.Persistence;
using Intergalaxy.Infrastructure.Data;

namespace Intergalaxy.Infrastructure.Persistence;

public class EFWriteRepository<T> : IWriteRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    public EFWriteRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(T entity)
        => await _dbContext.Set<T>().AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbContext.Set<T>().AddRangeAsync(entities);

    public void Remove(T entity)
        => _dbContext.Set<T>().Remove(entity);

    public void RemoveRange(IEnumerable<T> entities)
        => _dbContext.Set<T>().RemoveRange(entities);

    public void Update(T entity)
       => _dbContext.Set<T>().Update(entity);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);


}
