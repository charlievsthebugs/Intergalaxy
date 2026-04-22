using Intergalaxy.Application.CharacterRequests.Services;
using Intergalaxy.Application.Interfaces;
using Intergalaxy.Application.Persistence;
using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.CharacterRequests.Commands.CreateCharacterRequest;

public record CreateCharacterRequestCommand(
    int CharacterId,
    string Description, 
    string Requester,
    string EventName,
    DateTime EventDate
) : IRequest<int>;


public class CreateCharacterRequestCommandHandler : IRequestHandler<CreateCharacterRequestCommand, int>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IWriteRepository<CharacterRequest> _characterRequestRepository;
    private readonly RequestUniquenessChecker _requestUniquenessChecker;

    public CreateCharacterRequestCommandHandler(
        ICharacterRepository characterRepository,
        IWriteRepository<CharacterRequest> characterRequestRepository,
        RequestUniquenessChecker requestUniquenessChecker)
    {
        _characterRepository = characterRepository;
        _characterRequestRepository = characterRequestRepository;
        _requestUniquenessChecker = requestUniquenessChecker;
    }
    public async Task<int> Handle(CreateCharacterRequestCommand request, CancellationToken cancellationToken)
    {
        var characterExists = await _characterRepository
            .ExistAsync(request.CharacterId);

        if (!characterExists)
            throw new NotFoundException("Character does not exist", nameof(Character));

        // Ensure no approved request exists for the same character and event date
        await _requestUniquenessChecker.EnsureNoApprovedRequest(request.CharacterId, request.EventDate);

        var entity = CharacterRequest.Create(
            request.CharacterId,
            request.Description,
            request.Requester,
            request.EventName,
            request.EventDate
        );

        await _characterRequestRepository.AddAsync(entity);
        await _characterRequestRepository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

