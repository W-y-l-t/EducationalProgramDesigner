using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Subjects;

public interface INameBuilder
{
    IFormatBuilder WithName(TextUnit name);
}