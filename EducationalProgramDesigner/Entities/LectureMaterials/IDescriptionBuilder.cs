using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Entities.LectureMaterials;

public interface IDescriptionBuilder
{
    IDataBuilder WithDescription(Content description);
}
