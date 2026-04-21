using Intergalaxy.Domain.Entities;
using Intergalaxy.Domain.ValueObjects.Characters;
using Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi.ACL.Dtos;

namespace Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi.ACL.Translators;

public static class CharacterTranslator
{
    public static Character ToDomain(this CharacterDto dto)
    {
        return Character.Create(
            dto.Id,
            new CharacterName(dto.Name),
            new CharacterStatus(dto.Status),
            new CharacterSpecie(dto.Species),
            new CharacterGender(dto.Gender),
            new CharacterOrigin(dto.Origin?.Name ?? string.Empty),
            dto.Image
        );
    }
}
