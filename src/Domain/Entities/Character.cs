using Intergalaxy.Domain.Events.Character;
using Intergalaxy.Domain.ValueObjects.Characters;

namespace Intergalaxy.Domain.Entities;

public class Character : BaseAuditableEntity
{
    public int ExternalId { get; set; }
    public required CharacterName Name { get; set; }
    public required CharacterStatus Status { get; set; }
    public required CharacterSpecie Species { get; set; }
    public required CharacterOrigin Origin { get; set; }
    public required CharacterGender Gender { get; set; }
    public string Image { get; set; } = string.Empty;
    public DateTime ImportDate { get; set; } = DateTime.UtcNow;

    private Character() { }

    public static Character Create(
      int externalId,
      CharacterName name,
      CharacterStatus status,
      CharacterSpecie species,
      CharacterGender gender,
      CharacterOrigin origin,
      string image)
    {
        if (externalId <= 0)
            throw new ArgumentException("Invalid ExternalId");

        if (string.IsNullOrWhiteSpace(image))
            throw new ArgumentException("Image is required");

        var character = new Character
        {
            ExternalId = externalId,
            Name = name,
            Status = status,
            Species = species,
            Gender = gender,
            Origin = origin,
            Image = image
        };

        character.AddDomainEvent(new CharacterImportedEvent(externalId));

        return character;
    }
}
