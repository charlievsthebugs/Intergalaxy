using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Character> Characters { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
