using Intergalaxy.Application.Common.Models;
using Intergalaxy.Application.Mappings;
using Intergalaxy.Application.Persistence;
using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.Characters.Queries.GetCharacterByFilter;

public record GetCharacterByFilterQuery : IRequest<PaginatedList<CharacterPaginatedDto>>
{
    public string Name { get; init; } = string.Empty;
    public string? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public record GetCharacterByFilterQueryHandler : IRequestHandler<GetCharacterByFilterQuery, PaginatedList<CharacterPaginatedDto>>
{
    private readonly IReadRepository<Character> _context;
    private readonly IMapper _mapper;

    public GetCharacterByFilterQueryHandler(IReadRepository<Character> context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<CharacterPaginatedDto>> Handle(GetCharacterByFilterQuery request, CancellationToken cancellationToken)
    {
        var query = _context.GetAll().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(c => c.Name.Value.Contains(request.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(c => c.Status.Value == request.Status);
        }

        return await query.ProjectTo<CharacterPaginatedDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
