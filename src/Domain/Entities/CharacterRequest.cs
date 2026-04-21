namespace Intergalaxy.Domain.Entities;

public class CharacterRequest : BaseAuditableEntity
{
    public string? ExternalId { get; set; }
    public int CharacterId { get; set; }
    public int RequesterId { get; set; }
    public string EventName { get; private set; } = null!;
    public DateTime EventDate { get; private set; }
    public RequestStatus Status { get; private set; }
    public string? RejectionReason { get; private set; }

    public required Character Character { get; set; }

    public void StartReview()
    {
        EnsureTransition(RequestStatus.InProgress);

        Status = RequestStatus.InProgress;
        LastModified = DateTime.UtcNow;
    }

    public void Approve()
    {
        EnsureTransition(RequestStatus.Approved);

        Status = RequestStatus.Approved;
        LastModified = DateTime.UtcNow;
    }

    public void Reject(string reason)
    {
        EnsureTransition(RequestStatus.Rejected);

        if (string.IsNullOrWhiteSpace(reason))
            throw new Exception("Rejection reason is required");

        RejectionReason = reason;
        Status = RequestStatus.Rejected;
        LastModified = DateTime.UtcNow;
    }

    #region state machine 
    private static readonly Dictionary<RequestStatus, RequestStatus[]> ValidTransitions =
    new()
    {
        { RequestStatus.Pending, new[] { RequestStatus.InProgress, RequestStatus.Rejected } },
        { RequestStatus.InProgress, new[] { RequestStatus.Approved, RequestStatus.Rejected } },
        { RequestStatus.Approved, Array.Empty<RequestStatus>() },
        { RequestStatus.Rejected, Array.Empty<RequestStatus>() }
    };

    private void EnsureTransition(RequestStatus newStatus)
    {
        if (!ValidTransitions[Status].Contains(newStatus))
            throw new Exception($"Invalid transition from {Status} to {newStatus}");
    }

    #endregion
}
