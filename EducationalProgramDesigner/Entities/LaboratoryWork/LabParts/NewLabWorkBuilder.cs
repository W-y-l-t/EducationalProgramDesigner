using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;

public partial class LabWork
{
    public static INameBuilder NewBuilder(IRepository<LabWork> repository)
    {
        return new NewLabWorkBuilder(repository);
    }

    private class NewLabWorkBuilder : INameBuilder,
        IDescriptionBuilder, IEvaluationCriteriaBuilder, IWorthBuilder,
        IAuthorBuilder, IIdentifierBuilder
    {
        private readonly IRepository<LabWork> _repository;
        private Identifier _id;
        private TextUnit _name = new();
        private Content _description = new();
        private Content _criteria = new();
        private Score _worth;
        private User _author = new();

        public NewLabWorkBuilder(IRepository<LabWork> repository)
        {
            _repository = repository;
        }

        public IDescriptionBuilder WithName(TextUnit name)
        {
            _name = name;

            return this;
        }

        public IEvaluationCriteriaBuilder WithDescription(Content description)
        {
            _description = description;

            return this;
        }

        public IWorthBuilder WithEvaluationCriteria(Content criteria)
        {
            _criteria = criteria;

            return this;
        }

        public IAuthorBuilder WithWorth(Score worth)
        {
            _worth = worth;

            return this;
        }

        public IIdentifierBuilder WithAuthor(User author)
        {
            _author = author;

            return this;
        }

        public IIdentifierBuilder WithIdentifier(Identifier identifier)
        {
            _id = identifier;

            return this;
        }

        public LabWork Build()
        {
            var labWork = new LabWork(_id, _name, _description, _criteria, _worth, _author, null);
            _repository.AddEntity(labWork);

            return labWork;
        }
    }
}