namespace EducationalProgramDesigner.ValueObjects;

public readonly struct Identifier : IEquatable<Identifier>, IPrototype<Identifier>
{
    private readonly Guid _value;

    public Identifier()
    {
        _value = Guid.NewGuid();
    }

    public Identifier(Guid value)
    {
        _value = value;
    }

    public Identifier(string value)
    {
        _value = Guid.TryParse(value, out Guid result)
            ? result
            : throw new ArgumentException("The ID's value is not a valid GUID.");
    }

    public static bool operator ==(Identifier id1, Identifier id2)
    {
        return id1.Equals(id2);
    }

    public static bool operator !=(Identifier id1, Identifier id2)
    {
        return !id1.Equals(id2);
    }

    public bool Equals(Identifier other)
    {
        return _value.Equals(other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Identifier identifier && Equals(identifier);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public Identifier Clone()
    {
        return new Identifier(_value);
    }
}
