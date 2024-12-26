using EducationalProgramDesigner.StringContent;

namespace EducationalProgramDesigner.Entities.LaboratoryWork;

public interface IEvaluationCriteriaBuilder
{
    IWorthBuilder WithEvaluationCriteria(Content criteria);
}