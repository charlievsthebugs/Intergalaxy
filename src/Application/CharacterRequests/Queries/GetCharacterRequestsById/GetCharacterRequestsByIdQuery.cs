using Intergalaxy.Application.Characters.Queries.GetCharacterById;
using Intergalaxy.Application.Interfaces;
using Intergalaxy.Application.Persistence;
using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.CharacterRequests.Queries.GetCharacterRequestsById;

public record GetCharacterRequestsByIdQuery : IRequest<CharacterRequestDto?>
{
    public int Id { get; init; }
}

public class GetCharacterRequestsByIdQueryHandler : IRequestHandler<GetCharacterRequestsByIdQuery, CharacterRequestDto?>
{
    private readonly IReadRepository<CharacterRequest> _readRepository;
    private readonly IMapper _mapper;

    public GetCharacterRequestsByIdQueryHandler(
        IReadRepository<CharacterRequest> readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }
    public Task<CharacterRequestDto?> Handle(GetCharacterRequestsByIdQuery request, CancellationToken cancellationToken)
    {
        var character = _readRepository
           .FindAsQueryable(p => p.Id == request.Id)
           .AsNoTracking()
           .ProjectTo<CharacterRequestDto>(_mapper.ConfigurationProvider)
           .FirstOrDefault();

        return Task.FromResult(character);
    }
}
