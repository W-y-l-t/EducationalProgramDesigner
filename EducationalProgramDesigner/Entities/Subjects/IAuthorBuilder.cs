using EducationalProgramDesigner.Entities.Person;

namespace EducationalProgramDesigner.Entities.Subjects;

public interface IAuthorBuilder
{
    IIdentifierBuilder WithAuthor(User author);
}