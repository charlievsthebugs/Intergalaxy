namespace Intergalaxy.Domain.ValueObjects.Characters;

public sealed record CharacterGender
{
    public string Value { get; }

    private static readonly string[] Allowed =
        ["Female", "Male", "Genderless", "unknown"];

    public CharacterGender(string value)
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
