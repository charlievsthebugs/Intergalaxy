namespace Intergalaxy.Domain.ValueObjects.Characters;

public sealed record CharacterStatus
{
    public string Value { get; }

    private static readonly string[] Allowed =
        ["Alive", "Dead", "unknown"];

    public CharacterStatus(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            Value = "unknown";
        else if (!Allowed.Contains(value))
            throw new ArgumentException($"Invalid status: {value}");
        else
            Value = value;
    }

    public override string ToString() => Value;
}
