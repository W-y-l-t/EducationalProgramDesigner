using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Entities.Subjects.SubjectParts;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.EducationalPrograms.EducationalProgramParts;

public partial class EducationalProgram : IIdentifier
{
    private readonly Dictionary<Semester, List<Subject>> _subjects;

    private EducationalProgram(
        TextUnit name, Identifier id, User director, Dictionary<Semester, List<Subject>> subjects)
    {
        Name = name;
        Id = id;
        Director = director;
        _subjects = subjects;
    }

    public TextUnit Name { get; }

    public Identifier Id { get; }

    public User Director { get; }

    public IReadOnlyCollection<Subject> GetSubjectsInSemester(Semester semester)
    {
        return _subjects[semester];
    }
}
