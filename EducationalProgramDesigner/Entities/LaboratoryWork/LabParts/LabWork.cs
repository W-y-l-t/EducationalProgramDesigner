using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.ResultTypes;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;

public partial class LabWork : IIdentifier
{
    private LabWork(
        Identifier id,
        TextUnit name,
        Content description,
        Content criteria,
        Score worth,
        User author,
        Identifier? baseLabId)
    {
        Id = id;
        Name = name;
        Description = description;
        Criteria = criteria;
        Worth = worth;
        Author = author;
        BaseLabId = baseLabId;
    }

    public Identifier Id { get; }

    public TextUnit Name { get; private set; }

    public Content Description { get; private set; }

    public Content Criteria { get; private set; }

    public Score Worth { get; }

    public User Author { get; }

    public Identifier? BaseLabId { get; }

    public LabWork CloneInto(IRepository<LabWork> repository)
    {
        var labWork =
            new LabWork(
                new Identifier(),
                Name.Clone(),
                Description.Clone(),
                Criteria.Clone(),
                Worth.Clone(),
                Author.Clone(),
                Id.Clone());
        repository.AddEntity(labWork);

        return labWork;
    }

    public ChangingFieldsResult ChangeName(User editor, TextUnit newName)
    {
        return ChangeField(editor, newName, value => Name = value);
    }

    public ChangingFieldsResult ChangeDescription(User editor, Content newDescription)
    {
        return ChangeField(editor, newDescription, value => Description = value);
    }

    public ChangingFieldsResult ChangeCriteria(User editor, Content newCriteria)
    {
        return ChangeField(editor, newCriteria, value => Criteria = value);
    }

    private ChangingFieldsResult ChangeField<T>(User editor, T newValue, Action<T> updateAction)
    {
        if (editor != Author)
            return new ChangingFieldsResult.EditorIsNotTheAuthor();

        updateAction(newValue);
        return new ChangingFieldsResult.Success();
    }
}
