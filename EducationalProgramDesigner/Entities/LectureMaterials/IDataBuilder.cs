using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Entities.LectureMaterials;

public interface IDataBuilder
{
    IAuthorBuilder WithData(Content data);
}