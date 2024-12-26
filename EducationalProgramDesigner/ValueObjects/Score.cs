namespace EducationalProgramDesigner.ValueObjects;

public struct Score : IPrototype<Score>
{
    public Score(float score)
    {
        if (score is < 0 or > 100)
            throw new ArgumentException("Score must be between 0 and 100.");

        Value = score;
    }

    public float Value { get; }

    public Score Clone()
    {
        return new Score(Value);
    }
}