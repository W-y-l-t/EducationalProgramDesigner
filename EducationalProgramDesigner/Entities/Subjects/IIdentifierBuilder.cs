using EducationalProgramDesigner.Entities.Subjects.SubjectParts;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.Subjects;

public interface IIdentifierBuilder
{
    IIdentifierBuilder WithIdentifier(Identifier identifier);

    Subject Build();
}