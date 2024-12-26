namespace EducationalProgramDesigner.StringContent;

public class TextUnit(string value) : IPrototype<TextUnit>
{
    public TextUnit() : this(string.Empty) { }

    public string Value { get; } = value;

    public TextUnit Clone()
    {
        return new TextUnit(Value);
    }
}
