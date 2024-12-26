using EducationalProgramDesigner.Repository;

namespace EducationalProgramDesigner.Entities.EducationalPrograms.EducationalProgramParts;

public class EducationalProgramBuilderFactory
{
    private readonly IRepository<EducationalProgram> _repository;

    public EducationalProgramBuilderFactory(IRepository<EducationalProgram> repository)
    {
        _repository = repository;
    }

    public INameBuilder CreateBuilder()
    {
        return EducationalProgram.Builder(_repository);
    }
}