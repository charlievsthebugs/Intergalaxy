namespace Intergalaxy.Domain.ValueObjects.Characters;

public sealed record CharacterSpecie
{
    public string Value { get; }

    public CharacterSpecie(string value)
    {
        Value = string.IsNullOrWhiteSpace(value)
            ? "Unknown"
            : value.Trim();
    }

    public override string ToString() => Value;
}
