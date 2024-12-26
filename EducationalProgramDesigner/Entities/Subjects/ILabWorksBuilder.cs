using EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;

namespace EducationalProgramDesigner.Entities.Subjects;

public interface ILabWorksBuilder
{
    ILectureMaterialsBuilder WithLabWorks(IReadOnlyCollection<LabWork> labWorks);
}