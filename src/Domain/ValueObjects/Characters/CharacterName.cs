namespace Intergalaxy.Domain.ValueObjects.Characters;

public sealed record CharacterName
{
    public string Value { get; }

    public CharacterName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name is required");

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
