using Intergalaxy.Domain.Exceptions;

namespace Intergalaxy.Domain.ValueObjects.CharacterRequests;

public class Requester : ValueObject
{
    public string Value { get; }

    private Requester(string value)
    {
        Value = value;
    }

    public static Requester Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(["Requester is required"]);

        if (value.Length > 100)
            throw new DomainException(["Requester too long"]);

        return new Requester(value.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        //carga perezosa 
        yield return Value;
    }

    public override string ToString() => Value;
}
