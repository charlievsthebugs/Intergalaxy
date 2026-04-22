using Intergalaxy.Application.Common.Models;
using Intergalaxy.Application.Mappings;
using Intergalaxy.Application.Persistence;
using Intergalaxy.Domain.Entities;
using Intergalaxy.Domain.Enums;

namespace Intergalaxy.Application.CharacterRequests.Queries.GetCharacterRequestsByFilter;

public record GetCharacterRequestsByFilterQuery : IRequest<PaginatedList<CharacterRequestsPaginatedDto>>
{
    public RequestStatus? Status { get; init; }
    public int? CharacterId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCharacterRequestsByFilterQueryHandler : IRequestHandler<GetCharacterRequestsByFilterQuery, PaginatedList<CharacterRequestsPaginatedDto>>
{
    private readonly IReadRepository<CharacterRequest> _repository;
    private readonly IMapper _mapper;

    public GetCharacterRequestsByFilterQueryHandler(
        IReadRepository<CharacterRequest> repository,
        IMapper mapper
        )
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<CharacterRequestsPaginatedDto>> Handle(GetCharacterRequestsByFilterQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetAll().AsNoTracking();

        if (request.Status.HasValue)
        {
            query = query.Where(c => c.Status == request.Status);
        }

        if (request.CharacterId.HasValue)
        {
            query = query.Where(c => c.CharacterId == request.CharacterId);
        }

        return await query.ProjectTo<CharacterRequestsPaginatedDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
