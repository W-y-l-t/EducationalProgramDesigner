using EducationalProgramDesigner.Entities.EducationalPrograms.EducationalProgramParts;
using EducationalProgramDesigner.Entities.Subjects.SubjectParts;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.EducationalPrograms;

public interface ISubjectsBuilder
{
    ISubjectsBuilder AddSubjectToSemester(Semester semester, Subject subject);

    ISubjectsBuilder SetSubjectsToSemester(Semester semester, IEnumerable<Subject> subjects);

    EducationalProgram Build();
}