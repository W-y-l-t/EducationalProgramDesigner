using EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;

namespace EducationalProgramDesigner.Entities.LectureMaterials;

public interface IBasedOfBuilder
{
    IAuthorBuilder BasedOf(LectureMaterial lectureMaterial);
}