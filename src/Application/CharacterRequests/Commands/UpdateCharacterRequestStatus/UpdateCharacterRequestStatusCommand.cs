using Intergalaxy.Application.Persistence;
using Intergalaxy.Domain.Entities;
using Intergalaxy.Domain.Enums;

namespace Intergalaxy.Application.CharacterRequests.Commands.UpdateCharacterRequestStatus;

public record UpdateCharacterRequestStatusCommand : IRequest
{
    public int RequestId { get; init; }
    public RequestStatus NewStatus { get; init; }
}

public class UpdateCharacterRequestStatusCommandHandler : IRequestHandler<UpdateCharacterRequestStatusCommand>
{
    private readonly IReadRepository<CharacterRequest> _readRepository;
    private readonly IWriteRepository<CharacterRequest> _writeRepository;

    public UpdateCharacterRequestStatusCommandHandler(
        IReadRepository<CharacterRequest> readRepository,
        IWriteRepository<CharacterRequest> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }
    public async Task Handle(UpdateCharacterRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var characterRequest = _readRepository.GetAll()
            .FirstOrDefault(p => p.Id == request.RequestId);

        if (characterRequest is null)
            throw new NotFoundException("Character request not found", nameof(CharacterRequest));

        characterRequest.ChangeStatus(request.NewStatus);

        await _writeRepository.SaveChangesAsync(cancellationToken);
    }
}
