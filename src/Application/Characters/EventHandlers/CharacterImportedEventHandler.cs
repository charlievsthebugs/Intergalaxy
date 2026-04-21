using Intergalaxy.Domain.Events.Character;

namespace Intergalaxy.Application.Characters.EventHandlers;

public class CharacterImportedEventHandler
    : INotificationHandler<CharacterImportedEvent>
{
    public Task Handle(CharacterImportedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Character imported: {notification.ExternalId}");

        // - analytics
        // - logging
        // - cache invalidation

        return Task.CompletedTask;
    }
}
