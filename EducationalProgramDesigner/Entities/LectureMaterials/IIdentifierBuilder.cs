using EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LectureMaterials;

public interface IIdentifierBuilder
{
    IIdentifierBuilder WithIdentifier(Identifier identifier);

    LectureMaterial Build();
}