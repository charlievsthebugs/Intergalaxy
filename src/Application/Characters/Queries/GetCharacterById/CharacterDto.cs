using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.Characters.Queries.GetCharacterById;

public class CharacterDto
{
    public int ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public DateTime ImportDate { get; set; } = DateTime.UtcNow;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Character, CharacterDto>();
        }
    }
}
