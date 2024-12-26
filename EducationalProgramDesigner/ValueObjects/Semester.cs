namespace EducationalProgramDesigner.ValueObjects;

public struct Semester : IPrototype<Semester>, IEquatable<Semester>
{
    public Semester(int value)
    {
        if (value is < 0 or > 8)
            throw new ArgumentException("Semester must be between 0 and 8.");

        Value = value;
    }

    public int Value { get; }

    public bool Equals(Semester other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Identifier identifier && Equals(identifier);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public Semester Clone()
    {
        return new Semester(Value);
    }
}