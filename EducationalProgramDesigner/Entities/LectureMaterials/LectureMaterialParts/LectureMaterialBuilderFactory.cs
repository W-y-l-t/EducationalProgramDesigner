using EducationalProgramDesigner.Repository;

namespace EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;

public class LectureMaterialBuilderFactory
{
    private readonly IRepository<LectureMaterial> _repository;

    public LectureMaterialBuilderFactory(IRepository<LectureMaterial> repository)
    {
        _repository = repository;
    }

    public INameBuilder CreateNewBuilder()
    {
        return LectureMaterial.NewBuilder(_repository);
    }

    public IBasedOfBuilder CreateBasedOfBuilder()
    {
        return LectureMaterial.BasedOfBuilder(_repository);
    }
}