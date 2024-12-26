using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Entities.LectureMaterials;

public interface INameBuilder
{
    IDescriptionBuilder WithName(TextUnit name);
}