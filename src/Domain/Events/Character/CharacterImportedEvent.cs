namespace Intergalaxy.Domain.Events.Character;

public class CharacterImportedEvent : BaseEvent
{
    public int ExternalId { get; }

    public CharacterImportedEvent(int externalId)
    {
        ExternalId = externalId;
    }
}
