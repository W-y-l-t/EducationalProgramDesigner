using EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;

namespace EducationalProgramDesigner.Entities.LaboratoryWork;

public interface IBasedOfBuilder
{
    IAuthorBuilder BasedOf(LabWork labWork);
}