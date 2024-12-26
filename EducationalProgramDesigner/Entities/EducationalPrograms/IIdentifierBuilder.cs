using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.EducationalPrograms;

public interface IIdentifierBuilder
{
    ISubjectsBuilder WithIdentifier(Identifier id);
}