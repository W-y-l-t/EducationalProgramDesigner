using EducationalProgramDesigner.Entities.Person;

namespace EducationalProgramDesigner.Entities.LaboratoryWork;

public interface IAuthorBuilder
{
    IIdentifierBuilder WithAuthor(User user);
}