using EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;
using EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;
using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.Subjects.SubjectParts;

public partial class Subject
{
    public static INameBuilder NewBuilder(IRepository<Subject> repository)
    {
        return new NewSubjectBuilder(repository);
    }

    private class NewSubjectBuilder : INameBuilder,
        IAuthorBuilder, IIdentifierBuilder, IFormatBuilder,
        ILabWorksBuilder, ILectureMaterialsBuilder
    {
        private readonly IRepository<Subject> _repository;
        private IReadOnlyCollection<LabWork> _labWorks = [];
        private IReadOnlyCollection<LectureMaterial> _lectureMaterials = [];
        private Identifier _id;
        private TextUnit _name = new();
        private User _author = new();
        private Format _format;
        private Score _score;

        public NewSubjectBuilder(IRepository<Subject> repository)
        {
            _repository = repository;
        }

        public IFormatBuilder WithName(TextUnit name)
        {
            _name = name;

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

        public ILabWorksBuilder WithExam(Score score)
        {
            _score = score;
            _format = Format.Exam;

            return this;
        }

        public ILabWorksBuilder WithPass(Score score)
        {
            _score = score;
            _format = Format.Pass;

            return this;
        }

        public ILectureMaterialsBuilder WithLabWorks(IReadOnlyCollection<LabWork> labWorks)
        {
            _labWorks = [.. labWorks];

            return this;
        }

        public IAuthorBuilder WithLectureMaterials(IReadOnlyCollection<LectureMaterial> lectureMaterials)
        {
            _lectureMaterials = [.. lectureMaterials];

            return this;
        }

        public Subject Build()
        {
            var subject =
                new Subject(_id, _name, _labWorks, _lectureMaterials, _author, _format, _score, null);
            _repository.AddEntity(subject);

            return subject;
        }
    }
}