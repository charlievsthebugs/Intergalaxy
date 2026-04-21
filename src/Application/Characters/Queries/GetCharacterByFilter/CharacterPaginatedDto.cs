using System;
using System.Collections.Generic;
using System.Text;
using Intergalaxy.Application.Characters.Queries.GetCharacterById;
using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.Characters.Queries.GetCharacterByFilter;

public class CharacterPaginatedDto
{
    public int ExternalId { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public required string Species { get; set; }
    public required string Origin { get; set; }
    public required string Gender { get; set; }
    public string Image { get; set; } = string.Empty;
    public DateTime ImportDate { get; set; } = DateTime.UtcNow;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Character, CharacterPaginatedDto>();
        }
    }
}
