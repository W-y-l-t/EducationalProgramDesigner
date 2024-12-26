using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LaboratoryWork;

public interface IWorthBuilder
{
    IAuthorBuilder WithWorth(Score worth);
}