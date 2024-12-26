using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Entities.Subjects;

public interface INameBuilder
{
    IFormatBuilder WithName(TextUnit name);
}