using EducationalProgramDesigner.Entities.Person;

namespace EducationalProgramDesigner.Entities.LectureMaterials;

public interface IAuthorBuilder
{
    IIdentifierBuilder WithAuthor(User author);
}