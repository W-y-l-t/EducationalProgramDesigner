using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;

public partial class LectureMaterial
{
    public static INameBuilder NewBuilder(IRepository<LectureMaterial> repository)
    {
        return new NewLectureMaterialBuilder(repository);
    }

    private class NewLectureMaterialBuilder : INameBuilder,
        IDescriptionBuilder, IDataBuilder,
        IAuthorBuilder, IIdentifierBuilder
    {
        private readonly IRepository<LectureMaterial> _repository;
        private Identifier _id;
        private TextUnit _name = new();
        private Content _description = new();
        private Content _data = new();
        private User _author = new();

        public NewLectureMaterialBuilder(IRepository<LectureMaterial> repository)
        {
            _repository = repository;
        }

        public IDescriptionBuilder WithName(TextUnit name)
        {
            _name = name;

            return this;
        }

        public IDataBuilder WithDescription(Content description)
        {
            _description = description;

            return this;
        }

        public IAuthorBuilder WithData(Content data)
        {
            _data = data;

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

        public LectureMaterial Build()
        {
            var lectureMaterial = new LectureMaterial(_id, _name, _description, _data, _author, null);
            _repository.AddEntity(lectureMaterial);

            return lectureMaterial;
        }
    }
}