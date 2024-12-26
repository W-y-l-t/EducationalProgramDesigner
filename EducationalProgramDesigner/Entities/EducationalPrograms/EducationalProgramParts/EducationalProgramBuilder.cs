using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Entities.Subjects.SubjectParts;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.EducationalPrograms.EducationalProgramParts;

public partial class EducationalProgram
{
    public static INameBuilder Builder(IRepository<EducationalProgram> repository)
    {
        return new EducationalProgramBuilder(repository);
    }

    private class EducationalProgramBuilder : INameBuilder, IDirectorBuilder, IIdentifierBuilder, ISubjectsBuilder
    {
        private readonly IRepository<EducationalProgram> _repository;
        private readonly Dictionary<Semester, List<Subject>> _subjects = [];
        private TextUnit _name = new();
        private Identifier _id;
        private User _director = new();

        public EducationalProgramBuilder(IRepository<EducationalProgram> repository)
        {
            _repository = repository;
        }

        public IDirectorBuilder WithName(TextUnit name)
        {
            _name = name;

            return this;
        }

        public IIdentifierBuilder WithDirector(User director)
        {
            _director = director;

            return this;
        }

        public ISubjectsBuilder WithIdentifier(Identifier id)
        {
            _id = id;

            return this;
        }

        public ISubjectsBuilder AddSubjectToSemester(Semester semester, Subject subject)
        {
            if (_subjects.TryGetValue(semester, out List<Subject>? value))
            {
                value.Add(subject);
            }
            else
            {
                var newRepository = new List<Subject> { subject };

                _subjects[semester] = newRepository;
            }

            return this;
        }

        public ISubjectsBuilder SetSubjectsToSemester(Semester semester, IEnumerable<Subject> subjects)
        {
            _subjects[semester] = [.. subjects];

            return this;
        }

        public EducationalProgram Build()
        {
            var educationalProgram = new EducationalProgram(_name, _id, _director, _subjects);
            _repository.AddEntity(educationalProgram);

            return educationalProgram;
        }
    }
}