using EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;
using EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;
using EducationalProgramDesigner.Entities.Subjects.SubjectParts;
using EducationalProgramDesigner.Repository;

namespace EducationalProgramDesigner.Entities.Subjects;

public interface IBasedOfBuilder
{
    IAuthorBuilder BasedOf(
        Subject subject,
        IRepository<LabWork> labWorksRepository,
        IRepository<LectureMaterial> lectureMaterialsRepository);
}