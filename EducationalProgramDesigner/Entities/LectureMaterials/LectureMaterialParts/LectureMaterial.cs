using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.ResultTypes;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;

public partial class LectureMaterial : IIdentifier
{
    private LectureMaterial(
        Identifier id, TextUnit name, Content description, Content data, User author, Identifier? baseLectureMaterialId)
    {
        Id = id;
        Name = name;
        Description = description;
        Data = data;
        Author = author;
        BaseLectureMaterialId = baseLectureMaterialId;
    }

    public Identifier Id { get; }

    public TextUnit Name { get; private set; }

    public Content Description { get; private set; }

    public Content Data { get; private set; }

    public User Author { get; }

    public Identifier? BaseLectureMaterialId { get; }

    public LectureMaterial CloneInto(IRepository<LectureMaterial> repository)
    {
        var clonedLectureMaterial =
            new LectureMaterial(
                new Identifier(),
                Name.Clone(),
                Description.Clone(),
                Data.Clone(),
                Author.Clone(),
                Id.Clone());
        repository.AddEntity(clonedLectureMaterial);

        return clonedLectureMaterial;
    }

    public ChangingFieldsResult ChangeName(User editor, TextUnit newName)
    {
        return ChangeField(editor, newName, value => Name = value);
    }

    public ChangingFieldsResult ChangeDescription(User editor, Content newDescription)
    {
        return ChangeField(editor, newDescription, value => Description = value);
    }

    public ChangingFieldsResult ChangeData(User editor, Content newData)
    {
        return ChangeField(editor, newData, value => Data = value);
    }

    private ChangingFieldsResult ChangeField<T>(User editor, T newValue, Action<T> updateAction)
    {
        if (editor != Author)
            return new ChangingFieldsResult.EditorIsNotTheAuthor();

        updateAction(newValue);
        return new ChangingFieldsResult.Success();
    }
}
