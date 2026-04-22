using Intergalaxy.Domain.Exceptions;

namespace Intergalaxy.Domain.ValueObjects.CharacterRequests;

public class EventName : ValueObject
{
    public string Value { get; }

    private EventName(string value)
    {
        Value = value;
    }

    public static EventName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(["EventName is required"]);

        if (value.Length > 150)
            throw new DomainException(["EventName too long"]);

        return new EventName(value.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
