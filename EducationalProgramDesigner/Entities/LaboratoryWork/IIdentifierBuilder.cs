using EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LaboratoryWork;

public interface IIdentifierBuilder
{
    IIdentifierBuilder WithIdentifier(Identifier identifier);

    LabWork Build();
}