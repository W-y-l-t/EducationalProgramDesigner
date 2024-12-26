using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.Subjects;

public interface IFormatBuilder
{
    ILabWorksBuilder WithExam(Score score);

    ILabWorksBuilder WithPass(Score score);
}