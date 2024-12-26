using EducationalProgramDesigner.Entities.Person;

namespace EducationalProgramDesigner.Entities.EducationalPrograms;

public interface IDirectorBuilder
{
    IIdentifierBuilder WithDirector(User director);
}