using EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;
using EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;
using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.Subjects.SubjectParts;

public partial class Subject
{
    public static IBasedOfBuilder BasedOfBuilder(IRepository<Subject> repository)
    {
        return new BasedOfSubjectBuilder(repository);
    }

    private class BasedOfSubjectBuilder : IBasedOfBuilder, IAuthorBuilder, IIdentifierBuilder
    {
        private readonly IRepository<Subject> _repository;
        private IReadOnlyCollection<LabWork> _labWorks = [];
        private IReadOnlyCollection<LectureMaterial> _lectureMaterials = [];
        private Identifier _id;
        private TextUnit _name = new();
        private User _author = new();
        private Format _format;
        private Score _score;
        private Identifier? _baseSubjectId;

        public BasedOfSubjectBuilder(IRepository<Subject> repository)
        {
            _repository = repository;
        }

        public IAuthorBuilder BasedOf(
            Subject subject,
            IRepository<LabWork> labWorksRepository,
            IRepository<LectureMaterial> lectureMaterialsRepository)
        {
            _name = subject.Name.Clone();
            _format = subject.Format;
            _score = subject.Score.Clone();
            _labWorks = subject.LabWorks.Select(x => x.CloneInto(labWorksRepository)).ToList();
            _lectureMaterials =
                subject.LectureMaterials.Select(x => x.CloneInto(lectureMaterialsRepository)).ToList();
            _baseSubjectId = subject.Id.Clone();

            return this;
        }

        public IIdentifierBuilder WithAuthor(User author)
        {
            _author = author;

            return this;
        }

        public IIdentifierBuilder WithIdentifier(Identifier id)
        {
            _id = id;

            return this;
        }

        public Subject Build()
        {
            var subject =
                new Subject(_id, _name, _labWorks, _lectureMaterials, _author, _format, _score, _baseSubjectId);
            _repository.AddEntity(subject);

            return subject;
        }
    }
}