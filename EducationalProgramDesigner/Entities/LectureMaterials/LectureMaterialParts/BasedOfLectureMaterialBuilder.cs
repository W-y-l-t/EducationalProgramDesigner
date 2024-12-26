using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;

public partial class LectureMaterial
{
    public static IBasedOfBuilder BasedOfBuilder(IRepository<LectureMaterial> repository)
    {
        return new BasedOfLectureMaterialBuilder(repository);
    }

    private class BasedOfLectureMaterialBuilder : IAuthorBuilder, IIdentifierBuilder, IBasedOfBuilder
    {
        private readonly IRepository<LectureMaterial> _repository;
        private Identifier _id;
        private TextUnit _name = new();
        private Content _description = new();
        private Content _data = new();
        private User _author = new();
        private Identifier _basedOfLabWorkId;

        public BasedOfLectureMaterialBuilder(IRepository<LectureMaterial> repository)
        {
            _repository = repository;
        }

        public IAuthorBuilder BasedOf(LectureMaterial lectureMaterial)
        {
            _name = lectureMaterial.Name.Clone();
            _description = lectureMaterial.Description.Clone();
            _data = lectureMaterial.Data.Clone();
            _basedOfLabWorkId = lectureMaterial.Id.Clone();

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
            var lectureMaterial = new LectureMaterial(_id, _name, _description, _data, _author, _basedOfLabWorkId);
            _repository.AddEntity(lectureMaterial);

            return lectureMaterial;
        }
    }
}
