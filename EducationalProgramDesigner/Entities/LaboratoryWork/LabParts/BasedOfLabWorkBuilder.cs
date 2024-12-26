using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;

public partial class LabWork
{
    public static IBasedOfBuilder BasedOfBuilder(IRepository<LabWork> repository)
    {
        return new BasedOfLabWorkBuilder(repository);
    }

    private class BasedOfLabWorkBuilder : IAuthorBuilder, IIdentifierBuilder, IBasedOfBuilder
    {
        private readonly IRepository<LabWork> _repository;
        private Identifier _id;
        private TextUnit _name = new();
        private Content _description = new();
        private Content _criteria = new();
        private Score _worth;
        private User _author = new();
        private Identifier _basedOfLabWorkId;

        public BasedOfLabWorkBuilder(IRepository<LabWork> repository)
        {
            _repository = repository;
        }

        public IAuthorBuilder BasedOf(LabWork labWork)
        {
            _name = labWork.Name.Clone();
            _description = labWork.Description.Clone();
            _criteria = labWork.Criteria.Clone();
            _worth = labWork.Worth.Clone();
            _basedOfLabWorkId = labWork.Id.Clone();

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
            var labWork = new LabWork(_id, _name, _description, _criteria, _worth, _author, _basedOfLabWorkId);
            _repository.AddEntity(labWork);

            return labWork;
        }
    }
}
