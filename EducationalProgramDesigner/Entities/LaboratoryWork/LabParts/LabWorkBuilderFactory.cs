using EducationalProgramDesigner.Repository;

namespace EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;

public class LabWorkBuilderFactory
{
    private readonly IRepository<LabWork> _repository;

    public LabWorkBuilderFactory(IRepository<LabWork> repository)
    {
        _repository = repository;
    }

    public INameBuilder CreateNewBuilder()
    {
        return LabWork.NewBuilder(_repository);
    }

    public IBasedOfBuilder CreateBasedOfBuilder()
    {
        return LabWork.BasedOfBuilder(_repository);
    }
}