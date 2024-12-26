using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Entities.LaboratoryWork;

public interface INameBuilder
{
    IDescriptionBuilder WithName(TextUnit name);
}