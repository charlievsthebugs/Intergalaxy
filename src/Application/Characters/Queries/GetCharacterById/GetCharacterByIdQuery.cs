using Intergalaxy.Application.Persistence;
using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.Characters.Queries.GetCharacterById;

public record GetCharacterByIdQuery : IRequest<CharacterDto?>
{
    public int CharacterId { get; init; }
}


public class GetCharacterByIdQueryHandler : IRequestHandler<GetCharacterByIdQuery, CharacterDto?>
{
    private readonly IReadRepository<Character> _characterRepository;
    private readonly IMapper _mapper;
    public GetCharacterByIdQueryHandler(IReadRepository<Character> characterRepository, IMapper mapper)
    {
        _characterRepository = characterRepository;
        _mapper = mapper;
    }
    public Task<CharacterDto?> Handle(GetCharacterByIdQuery request, CancellationToken cancellationToken)
    {
        var character = _characterRepository
            .FindAsQueryable(p => p.Id == request.CharacterId)
            .AsNoTracking()
            .ProjectTo<CharacterDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault();

        return Task.FromResult(character);
    }
}
