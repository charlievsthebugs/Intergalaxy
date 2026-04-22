using Intergalaxy.Application.Persistence;
using Intergalaxy.Domain.Entities;
using Intergalaxy.Domain.Enums;
using Intergalaxy.Domain.Exceptions;

namespace Intergalaxy.Application.CharacterRequests.Services;

public class RequestUniquenessChecker
{
    private readonly IReadRepository<CharacterRequest> _repository;

    public RequestUniquenessChecker(IReadRepository<CharacterRequest> repository)
    {
        _repository = repository;
    }

    public async Task EnsureNoApprovedRequest(int characterId, DateTime eventDate)
    {
        var exists = await _repository
             .AnyAsync(x =>
                x.CharacterId == characterId &&
                x.EventDate.Date == eventDate.Date &&
                x.Status == RequestStatus.Approved);

        if (exists)
            throw new DomainException([
                "This character already has an approved request for this date"
            ]);
    }
}
