namespace EducationalProgramDesigner.StringContent;

public class Content : IPrototype<Content>
{
    public Content()
    {
        Parts = [];
        Title = new TextUnit();
    }

    private Content(IReadOnlyCollection<TextUnit> parts, TextUnit? title)
    {
        Parts = parts;
        Title = title;
    }

    public static ContentBuilder Builder => new ContentBuilder();

    public IReadOnlyCollection<TextUnit> Parts { get; }

    public TextUnit? Title { get; }

    public class ContentBuilder
    {
        private readonly List<TextUnit> _parts = [];
        private TextUnit? _title;

        public ContentBuilder AddPart(TextUnit part)
        {
            _parts.Add(part);

            return this;
        }

        public ContentBuilder AddTitle(TextUnit title)
        {
            _title = title;

            return this;
        }

        public Content Build()
        {
            return new Content(_parts, _title);
        }
    }

    public Content Clone()
    {
        var clonedParts = Parts.Select(part => part.Clone()).ToList();
        TextUnit? title = Title?.Clone();

        return new Content(clonedParts, title);
    }
}
