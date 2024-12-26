using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Entities.LaboratoryWork;

public interface IDescriptionBuilder
{
    IEvaluationCriteriaBuilder WithDescription(Content description);
}