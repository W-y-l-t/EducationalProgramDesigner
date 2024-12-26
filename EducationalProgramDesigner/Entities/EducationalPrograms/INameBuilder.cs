using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Entities.EducationalPrograms;

public interface INameBuilder
{
    IDirectorBuilder WithName(TextUnit name);
}