namespace Intergalaxy.Domain.ValueObjects.Characters;

public sealed record CharacterOrigin
{
    public string Value { get; }

    public CharacterOrigin(string? value)
    {
        Value = string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
    }
}
