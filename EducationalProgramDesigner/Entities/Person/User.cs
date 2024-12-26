using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Entities.Person;

public class User : IHuman, IIdentifier, IPrototype<User>
{
    public User()
    {
        Id = new Identifier();
        Name = string.Empty;
    }

    public User(Identifier id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Id = id;
        Name = name;
    }

    public Identifier Id { get; }

    public string Name { get; }

    public User Clone()
    {
        return new User(Id, Name);
    }
}
