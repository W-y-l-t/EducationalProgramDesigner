using EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;
using EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;
using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.ResultTypes;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.Subjects.SubjectParts;

public enum Format
{
    Exam,
    Pass,
}

public partial class Subject : IIdentifier
{
    private const float MaxScore = 100f;
    private readonly List<LabWork> _labWorks;
    private List<LectureMaterial> _lectureMaterials;

    private Subject(
        Identifier id,
        TextUnit name,
        IReadOnlyCollection<LabWork> labWorks,
        IReadOnlyCollection<LectureMaterial> materials,
        User author,
        Format format,
        Score score,
        Identifier? baseSubjectId)
    {
        Id = id;
        Name = name;
        _labWorks = [.. labWorks];
        _lectureMaterials = [.. materials];
        Author = author;
        Format = format;
        Score = score;
        BaseSubjectId = baseSubjectId;

        float totalScore = labWorks.Sum(lw => lw.Worth.Value) + (Format is Format.Exam ? score.Value : 0);
        if (Math.Abs(totalScore - MaxScore) > float.Epsilon)
            throw new ArgumentException("The total score is not equal to the max score.");
    }

    public Identifier Id { get; }

    public TextUnit Name { get; private set; }

    public IReadOnlyCollection<LabWork> LabWorks => _labWorks;

    public IReadOnlyCollection<LectureMaterial> LectureMaterials => _lectureMaterials;

    public User Author { get; }

    public Format Format { get; }

    public Score Score { get; private set; }

    public Identifier? BaseSubjectId { get; }

    public Subject CloneInto(
        IRepository<Subject> subjectsRepository,
        IRepository<LabWork> labWorksRepository,
        IRepository<LectureMaterial> lectureMaterialsRepository)
    {
        var clonedSubject =
            new Subject(
                Id.Clone(),
                Name.Clone(),
                LabWorks.Select(x => x.CloneInto(labWorksRepository)).ToList(),
                LectureMaterials.Select(x => x.CloneInto(lectureMaterialsRepository)).ToList(),
                Author.Clone(),
                Format,
                Score.Clone(),
                BaseSubjectId?.Clone());
        subjectsRepository.AddEntity(clonedSubject);

        return clonedSubject;
    }

    public ChangingFieldsResult AddLectureMaterial(User editor, LectureMaterial lectureMaterial)
    {
        return ChangeField(editor, lectureMaterial, value => _lectureMaterials.Add(value));
    }

    public ChangingFieldsResult ChangeName(User editor, TextUnit newName)
    {
        return ChangeField(editor, newName, value => Name = value);
    }

    public ChangingFieldsResult ChangeLectureMaterials(
        User editor, IReadOnlyCollection<LectureMaterial> newLectureMaterials)
    {
        return ChangeField(
            editor,
            newLectureMaterials,
            value => _lectureMaterials = [.. value]);
    }

    public ChangingFieldsResult ChangePassScore(User editor, Score newScore)
    {
        return
            newScore.Value > MaxScore
            ? new ChangingFieldsResult.MinimumScoreMustBeLessThenOneHundred()
            : ChangeField(editor, newScore, value => Score = value);
    }

    private ChangingFieldsResult ChangeField<T>(User editor, T newValue, Action<T> updateAction)
    {
        if (editor != Author)
            return new ChangingFieldsResult.EditorIsNotTheAuthor();

        updateAction(newValue);
        return new ChangingFieldsResult.Success();
    }
}