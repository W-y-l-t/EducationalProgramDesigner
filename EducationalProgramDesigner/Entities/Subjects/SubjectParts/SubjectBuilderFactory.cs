using EducationalProgramDesigner.Repository;

namespace EducationalProgramDesigner.Entities.Subjects.SubjectParts;

public class SubjectBuilderFactory
{
    private readonly IRepository<Subject> _repository;

    public SubjectBuilderFactory(IRepository<Subject> repository)
    {
        _repository = repository;
    }

    public INameBuilder CreateNewBuilder()
    {
        return Subject.NewBuilder(_repository);
    }

    public IBasedOfBuilder CreateBasedOfBuilder()
    {
        return Subject.BasedOfBuilder(_repository);
    }
}