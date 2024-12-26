using EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;

namespace EducationalProgramDesigner.Entities.Subjects;

public interface ILectureMaterialsBuilder
{
    IAuthorBuilder WithLectureMaterials(IReadOnlyCollection<LectureMaterial> materials);
}