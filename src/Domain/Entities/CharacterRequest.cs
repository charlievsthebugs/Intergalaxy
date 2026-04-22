using Intergalaxy.Domain.Exceptions;
using Intergalaxy.Domain.ValueObjects.CharacterRequests;

namespace Intergalaxy.Domain.Entities;

public class CharacterRequest : BaseAuditableEntity
{
    public int CharacterId { get; private set; }
    public Requester Requester { get; private set; } = null!;
    public EventName EventName { get; private set; } = null!;
    public DateTime EventDate { get; private set; }
    public RequestStatus Status { get; private set; }
    public string? RejectionReason { get; private set; }
    public string Description { get; private set; } = null!;

    public Character? Character { get; private set; }

    private CharacterRequest() { }
    private CharacterRequest(
        int characterId, 
        string description,
        Requester requester,
        EventName eventName, DateTime eventDate)
    {
        CharacterId = characterId;
        Description = description;
        Requester = requester;
        EventName = eventName;
        EventDate = eventDate;
        Status = RequestStatus.Pending;
    }


    public static CharacterRequest Create(int characterId, string description, string requester,
    string eventName, DateTime eventDate)
    {
        if (characterId <= 0)
            throw new DomainException(["invalid CharacterId"]);

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException(["Description is required"]);

        if(eventDate <= DateTime.UtcNow)
            throw new DomainException(["EventDate must be in the future"]);

        return new CharacterRequest(
            characterId, description, 
            Requester.Create(requester), 
            EventName.Create(eventName), eventDate);
    }

    public void ChangeStatus(RequestStatus newStatus)
    {
        if (!IsValidTransition(newStatus))
            throw new DomainException(["Invalid transition"]);

        Status = newStatus;
    }

    #region state machine 
    private bool IsValidTransition(RequestStatus newStatus)
    {
        return (Status, newStatus) switch
        {
            (RequestStatus.Pending, RequestStatus.InProgress) => true,
            (RequestStatus.InProgress, RequestStatus.Approved) => true,
            (RequestStatus.InProgress, RequestStatus.Rejected) => true,
            (RequestStatus.Pending, RequestStatus.Rejected) => true,
            _ => false
        };
    }
    #endregion
}
