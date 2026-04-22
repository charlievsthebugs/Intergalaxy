using Intergalaxy.Application.Common.Interfaces;
using Intergalaxy.Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Intergalaxy.Infrastructure.Persistence;

public class EFCharacterRepository : ICharacterRepository
{
    private readonly IApplicationDbContext _dbContext;

    public EFCharacterRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistAsync(int id)
    {
        return await _dbContext.Characters
             .AnyAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByExternalId(int externalId)
    {
        return await _dbContext.Characters
             .AnyAsync(c => c.ExternalId == externalId);
    }

    public async Task<List<int>> GetExistingExternalIdsAsync(IEnumerable<int> externalIds)
    {
        return await _dbContext.Characters
            .Where(x => externalIds.Contains(x.ExternalId))
            .Select(x => x.ExternalId)
            .ToListAsync();
    }
}
