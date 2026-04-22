using Intergalaxy.Application.Characters.Queries.GetCharacterByFilter;
using Intergalaxy.Domain.Entities;
using Intergalaxy.Domain.Enums;
using Intergalaxy.Domain.ValueObjects.CharacterRequests;

namespace Intergalaxy.Application.CharacterRequests.Queries.GetCharacterRequestsByFilter;

public class CharacterRequestsPaginatedDto
{
    public int CharacterId { get; private set; }
    public string Requester { get; private set; } = string.Empty;
    public string EventName { get; private set; } = string.Empty;
    public DateTime EventDate { get; private set; }
    public RequestStatus Status { get; private set; }
    public string? RejectionReason { get; private set; }
    public string Description { get; private set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CharacterRequest, CharacterRequestsPaginatedDto>();
               
        }
    }
}
