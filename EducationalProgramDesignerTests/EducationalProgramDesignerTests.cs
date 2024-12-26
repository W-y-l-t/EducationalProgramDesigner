using EducationalProgramDesigner.Entities.EducationalPrograms.EducationalProgramParts;
using EducationalProgramDesigner.Entities.LaboratoryWork.LabParts;
using EducationalProgramDesigner.Entities.LectureMaterials.LectureMaterialParts;
using EducationalProgramDesigner.Entities.Person;
using EducationalProgramDesigner.Entities.Subjects.SubjectParts;
using EducationalProgramDesigner.Repository;
using EducationalProgramDesigner.ResultTypes;
using EducationalProgramDesigner.StringContent;
using EducationalProgramDesigner.ValueObjects;
using Xunit;

namespace EducationalProgramDesignerTests;

public class Lab2Tests
{
    [Fact]
    public void Identifier_IDConstructor_CreatedSuccessfully()
    {
        // Arrange
        var guid = Guid.NewGuid();
        string stringGuid = "3f2504e0-4f89-11d3-9a0c-0305e82c3301";

        // Act
        Exception? exception1 = Record.Exception(() =>
            new User(new Identifier(guid), "Sonya Marmeladova"));
        Exception? exception2 = Record.Exception(() =>
            new User(new Identifier(stringGuid), "Chester Bennington"));

        // Assert
        Assert.Null(exception1);
        Assert.Null(exception2);
    }

    [Theory]
    [InlineData("")]
    [InlineData("hello-my-name-is-something")]
    [InlineData("eight-four-four-four-twelve")]
    [InlineData("17031703-1703-1703-1703-170317031703-1703")]
    [InlineData("879f0gjh-df4l-poo9-ni3g-qm0fr62kd72g")]
    public void Identifier_IDConstructor_NotCreated(string stringGuid)
    {
        // Arrange

        // Act
        Exception? exception = Record.Exception(() =>
            new User(new Identifier(stringGuid), "Spider Man"));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void StringContent_TextUnit_Clone()
    {
        // Arrange
        var textUnit1 = new TextUnit("Hello");
        var textUnit2 = new TextUnit("What's up");

        // Act
        TextUnit textUnit3 = textUnit1.Clone();
        TextUnit textUnit4 = textUnit2.Clone();

        // Assert
        Assert.NotEqual(textUnit1, textUnit3);
        Assert.NotEqual(textUnit2, textUnit4);
        Assert.Equal("Hello", textUnit3.Value);
        Assert.Equal("What's up", textUnit4.Value);
    }

    [Fact]
    public void StringContent_Content_Build()
    {
        // Arrange
        var title = new TextUnit("Greetings");
        var textUnit1 = new TextUnit("Hello");
        var textUnit2 = new TextUnit("What's up");
        var textUnit3 = new TextUnit("Make America Great Again");

        var textUnitList = new List<TextUnit> { textUnit1, textUnit2, textUnit3 };

        // Act
        Content content =
            Content.Builder
                .AddTitle(title)
                .AddPart(textUnit1)
                .AddPart(textUnit2)
                .AddPart(textUnit3)
                .Build();

        // Assert
        Assert.NotNull(content.Title);
        Assert.Equal(title.Value, content.Title.Value);
        Assert.Equal(textUnitList, content.Parts);
    }

    [Fact]
    public void StringContent_Content_Clone()
    {
        // Arrange
        var title = new TextUnit("Greetings");
        var textUnit1 = new TextUnit("Hello");
        var textUnit2 = new TextUnit("What's up");

        var textUnitList = new List<TextUnit> { textUnit1, textUnit2 };

        // Act
        Content content =
            Content.Builder
                .AddTitle(title)
                .AddPart(textUnit1)
                .AddPart(textUnit2)
                .Build();

        Content clonedContent = content.Clone();

        // Assert
        Assert.NotNull(clonedContent.Title);
        Assert.Equal("Greetings", clonedContent.Title.Value);
        Assert.All(
            clonedContent.Parts.Zip(textUnitList),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));
    }

    [Fact]
    public void Person_User_CorrectlyInit()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        Exception? exception = Record.Exception(() =>
            new User(new Identifier(guid), "Sonya Marmeladova"));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Repository_UserRepository_AllMethodsCorrect()
    {
        // Arrange
        var userRepository = new InMemoryRepository<User>();

        var id1 = new Identifier();
        var id2 = new Identifier();
        var id3 = new Identifier();

        var user1 = new User(id1, "Oleg");
        var user2 = new User(id2, "Pablo");
        var user3 = new User(id3, "Kate");

        // Act
        userRepository.AddEntity(user1);
        userRepository.AddEntity(user2);
        userRepository.AddEntity(user3);

        userRepository.RemoveEntity(user1);
        userRepository.RemoveEntity(id3);

        userRepository.AddEntity(user3);

        // Assert
        Assert.Null(userRepository.FindEntity(id1));
        Assert.NotNull(userRepository.FindEntity(id3));
        Assert.Equal("Pablo", userRepository.GetEntity(id2).Name);
        Assert.All(
            userRepository.GetEntities().Zip([user2, user3]),
            pair => Assert.Equal(pair.First.Id, pair.Second.Id));
    }

    [Fact]
    public void Repository_InMemoryRepository_CloneTest()
    {
        // Arrange
        var authorId = new Identifier();
        var lab1Id = new Identifier();
        var lab2Id = new Identifier();

        var author = new User(authorId, "Vlad");
        var labName = new TextUnit("lab-2-0-2-4");
        Content description =
            Content.Builder
                .AddTitle(new TextUnit("Description"))
                .AddPart(new TextUnit("Do"))
                .AddPart(new TextUnit("Don't"))
                .Build();
        Content criteria =
            Content.Builder
                .AddTitle(new TextUnit("Criteria"))
                .AddPart(new TextUnit("16 points for everyone"))
                .Build();
        var worth = new Score(33.24f);

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        LabWork labWork1 =
            labWorkFactory.CreateNewBuilder()
                .WithName(labName)
                .WithDescription(description)
                .WithEvaluationCriteria(criteria)
                .WithWorth(worth)
                .WithAuthor(author)
                .WithIdentifier(lab1Id)
                .Build();

        LabWork labWork2 =
            labWorkFactory.CreateNewBuilder()
                .WithName(labName)
                .WithDescription(description)
                .WithEvaluationCriteria(criteria)
                .WithWorth(worth)
                .WithAuthor(author)
                .WithIdentifier(lab2Id)
                .Build();

        // Act
        LabWork labWork3 = labWork1.CloneInto(labWorksRepository);

        // Assert
        Assert.NotEqual(labWork1, labWork3);
        Assert.Equal(labWork1.Id, labWork3.BaseLabId);
        Assert.Equal(labWork3, labWorksRepository.FindEntity(labWork3.Id));
        Assert.Equal(3, labWorksRepository.GetEntities().Count());
    }

    [Fact]
    public void LaboratoryWork_LabWork_NewBuild()
    {
        // Arrange
        var authorId = new Identifier();
        var labId = new Identifier();

        var author = new User(authorId, "Vlad");
        var labName = new TextUnit("lab-2-0-2-4");
        Content description =
            Content.Builder
                .AddTitle(new TextUnit("Description"))
                .AddPart(new TextUnit("Do"))
                .AddPart(new TextUnit("Don't"))
                .Build();
        Content criteria =
            Content.Builder
                .AddTitle(new TextUnit("Criteria"))
                .AddPart(new TextUnit("16 points for everyone"))
                .Build();
        var worth = new Score(33.24f);

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        // Act
        LabWork labWork =
            labWorkFactory.CreateNewBuilder()
                .WithName(labName)
                .WithDescription(description)
                .WithEvaluationCriteria(criteria)
                .WithWorth(worth)
                .WithAuthor(author)
                .WithIdentifier(labId)
                .Build();

        // Assert
        Assert.Null(labWork.BaseLabId);
        Assert.Equal(labId, labWork.Id);
        Assert.Equal(labName.Value, labWork.Name.Value);
        Assert.Equal(worth.Value, labWork.Worth.Value);
        Assert.Equal(author.Id, labWork.Author.Id);
        Assert.Equal(author.Name, labWork.Author.Name);
        Assert.NotNull(labWork.Description.Title);
        Assert.NotNull(description.Title);
        Assert.Equal(description.Title.Value, labWork.Description.Title.Value);
        Assert.All(
            labWork.Description.Parts.Zip(description.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));
        Assert.NotNull(labWork.Criteria.Title);
        Assert.NotNull(criteria.Title);
        Assert.Equal(criteria.Title.Value, labWork.Criteria.Title.Value);
        Assert.All(
            labWork.Criteria.Parts.Zip(criteria.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));

        Assert.Single(labWorksRepository.GetEntities());
        Assert.NotNull(labWorksRepository.FindEntity(labId));
    }

    [Fact]
    public void LaboratoryWork_LabWork_BasedOfBuild()
    {
        // Arrange
        var authorId = new Identifier();
        var labId = new Identifier();

        var author = new User(authorId, "Vlad");
        var labName = new TextUnit("lab-2-0-2-4");
        Content description =
            Content.Builder
                .AddTitle(new TextUnit("Description"))
                .AddPart(new TextUnit("Do"))
                .AddPart(new TextUnit("Don't"))
                .Build();
        Content criteria =
            Content.Builder
                .AddTitle(new TextUnit("Criteria"))
                .AddPart(new TextUnit("16 points for everyone"))
                .Build();
        var worth = new Score(33.24f);

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        LabWork labWork =
            labWorkFactory.CreateNewBuilder()
                .WithName(labName)
                .WithDescription(description)
                .WithEvaluationCriteria(criteria)
                .WithWorth(worth)
                .WithAuthor(author)
                .WithIdentifier(labId)
                .Build();

        var newAuthorId = new Identifier();
        var newLabId = new Identifier();

        var basedOfAuthor = new User(newAuthorId, "Base");

        // Act
        LabWork basedOfLabWork =
            labWorkFactory.CreateBasedOfBuilder()
                .BasedOf(labWork)
                .WithAuthor(basedOfAuthor)
                .WithIdentifier(newLabId)
                .Build();

        // Assert
        Assert.NotNull(basedOfLabWork.BaseLabId);
        Assert.Equal(labWork.Id, basedOfLabWork.BaseLabId);

        Assert.Equal(newLabId, basedOfLabWork.Id);
        Assert.Equal(labName.Value, basedOfLabWork.Name.Value);
        Assert.Equal(worth.Value, basedOfLabWork.Worth.Value);
        Assert.Equal(basedOfAuthor.Id, basedOfLabWork.Author.Id);
        Assert.Equal(basedOfAuthor.Name, basedOfLabWork.Author.Name);
        Assert.NotNull(basedOfLabWork.Description.Title);
        Assert.NotNull(description.Title);
        Assert.Equal(description.Title.Value, basedOfLabWork.Description.Title.Value);
        Assert.All(
            basedOfLabWork.Description.Parts.Zip(description.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));
        Assert.NotNull(basedOfLabWork.Criteria.Title);
        Assert.NotNull(criteria.Title);
        Assert.Equal(criteria.Title.Value, basedOfLabWork.Criteria.Title.Value);
        Assert.All(
            basedOfLabWork.Criteria.Parts.Zip(criteria.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));

        Assert.NotNull(labWorksRepository.FindEntity(labId));
        Assert.NotNull(labWorksRepository.FindEntity(newLabId));
        Assert.Equal(2, labWorksRepository.GetEntities().Count());
    }

    [Fact]
    public void LaboratoryWork_LabWork_ChangeFields()
    {
        // Arrange
        var authorId = new Identifier();
        var labId = new Identifier();

        var author = new User(authorId, "Vlad");
        var labName = new TextUnit("lab-2-0-2-4");
        Content description =
            Content.Builder
                .AddTitle(new TextUnit("Description"))
                .AddPart(new TextUnit("Do"))
                .AddPart(new TextUnit("Don't"))
                .Build();
        Content criteria =
            Content.Builder
                .AddTitle(new TextUnit("Criteria"))
                .AddPart(new TextUnit("16 points for everyone"))
                .Build();
        var worth = new Score(33.24f);

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        LabWork labWork =
            labWorkFactory.CreateNewBuilder()
                .WithName(labName)
                .WithDescription(description)
                .WithEvaluationCriteria(criteria)
                .WithWorth(worth)
                .WithAuthor(author)
                .WithIdentifier(labId)
                .Build();

        Content newDescription =
            Content.Builder
                .AddPart(new TextUnit("NEW DESCRIPTION"))
                .Build();
        Content newCriteria =
            Content.Builder
                .AddPart(new TextUnit("NEW CRITERIA"))
                .Build();

        var newName = new TextUnit("NEW NAME");
        var newUserId = new Identifier();
        var someUser = new User(newUserId, "Ronaldo");

        // Act
        ChangingFieldsResult changeNameResult = labWork.ChangeName(author, newName);
        ChangingFieldsResult changeDescriptionResult = labWork.ChangeDescription(author, newDescription);
        ChangingFieldsResult changeCriteriaResult1 = labWork.ChangeCriteria(someUser, newCriteria);
        ChangingFieldsResult changeCriteriaResult2 = labWork.ChangeCriteria(author, newCriteria);

        // Assert
        Assert.IsType<ChangingFieldsResult.Success>(changeNameResult);
        Assert.Equal(newName.Value, labWork.Name.Value);
        Assert.IsType<ChangingFieldsResult.Success>(changeDescriptionResult);
        Assert.Null(labWork.Description.Title);
        Assert.Null(newDescription.Title);
        Assert.All(
            labWork.Description.Parts.Zip(newDescription.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));
        Assert.IsType<ChangingFieldsResult.EditorIsNotTheAuthor>(changeCriteriaResult1);
        Assert.IsType<ChangingFieldsResult.Success>(changeCriteriaResult2);
        Assert.Null(labWork.Criteria.Title);
        Assert.Null(newCriteria.Title);
        Assert.All(
            labWork.Criteria.Parts.Zip(newCriteria.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));

        Assert.Null(labWork.BaseLabId);
        Assert.Equal(labId, labWork.Id);
        Assert.Equal(worth.Value, labWork.Worth.Value);
        Assert.Equal(author.Id, labWork.Author.Id);
        Assert.Equal(author.Name, labWork.Author.Name);
    }

    [Fact]
    public void LectureMaterials_LectureMaterial_NewBuild()
    {
        // Arrange
        var authorId = new Identifier();
        var lectureMaterialId = new Identifier();

        var author = new User(authorId, "Messi");
        var name = new TextUnit("OOP-OOP-OOP-OOP");
        Content description =
            Content.Builder
                .AddTitle(new TextUnit("Description"))
                .AddPart(new TextUnit("Do"))
                .AddPart(new TextUnit("Don't"))
                .Build();
        Content data =
            Content.Builder
                .AddTitle(new TextUnit("Content"))
                .AddPart(new TextUnit("Never gonna give you up"))
                .Build();

        var lectureMaterialsRepository = new InMemoryRepository<LectureMaterial>();
        var lectureMaterialsFactory = new LectureMaterialBuilderFactory(lectureMaterialsRepository);

        // Act
        LectureMaterial lectureMaterial =
            lectureMaterialsFactory.CreateNewBuilder()
                .WithName(name)
                .WithDescription(description)
                .WithData(data)
                .WithAuthor(author)
                .WithIdentifier(lectureMaterialId)
                .Build();

        // Assert
        Assert.Null(lectureMaterial.BaseLectureMaterialId);
        Assert.Equal(lectureMaterialId, lectureMaterial.Id);
        Assert.Equal(name.Value, lectureMaterial.Name.Value);
        Assert.Equal(author.Id, lectureMaterial.Author.Id);
        Assert.Equal(author.Name, lectureMaterial.Author.Name);
        Assert.NotNull(lectureMaterial.Description.Title);
        Assert.NotNull(description.Title);
        Assert.Equal(description.Title.Value, lectureMaterial.Description.Title.Value);
        Assert.All(
            lectureMaterial.Description.Parts.Zip(description.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));
        Assert.NotNull(lectureMaterial.Data.Title);
        Assert.NotNull(data.Title);
        Assert.Equal(data.Title.Value, lectureMaterial.Data.Title.Value);
        Assert.All(
            lectureMaterial.Data.Parts.Zip(data.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));

        Assert.Single(lectureMaterialsRepository.GetEntities());
        Assert.NotNull(lectureMaterialsRepository.FindEntity(lectureMaterialId));
    }

    [Fact]
    public void LectureMaterials_LectureMaterial_BasedOfBuild()
    {
        // Arrange
        var authorId = new Identifier();
        var lectureMaterialId = new Identifier();

        var author = new User(authorId, "Messi");
        var name = new TextUnit("OOP-OOP-OOP-OOP");
        Content description =
            Content.Builder
                .AddTitle(new TextUnit("Description"))
                .AddPart(new TextUnit("Do"))
                .AddPart(new TextUnit("Don't"))
                .Build();
        Content data =
            Content.Builder
                .AddTitle(new TextUnit("Content"))
                .AddPart(new TextUnit("Never gonna give you up"))
                .Build();

        var lectureMaterialsRepository = new InMemoryRepository<LectureMaterial>();
        var lectureMaterialsFactory = new LectureMaterialBuilderFactory(lectureMaterialsRepository);

        LectureMaterial lectureMaterial =
            lectureMaterialsFactory.CreateNewBuilder()
                .WithName(name)
                .WithDescription(description)
                .WithData(data)
                .WithAuthor(author)
                .WithIdentifier(lectureMaterialId)
                .Build();

        var basedOfAuthorId = new Identifier();

        var basedOfAuthor = new User(basedOfAuthorId, "Base");
        var basedLectureMaterialId = new Identifier();

        // Act
        LectureMaterial basedOfLectureMaterial =
            lectureMaterialsFactory.CreateBasedOfBuilder()
                .BasedOf(lectureMaterial)
                .WithAuthor(basedOfAuthor)
                .WithIdentifier(basedLectureMaterialId)
                .Build();

        // Assert
        Assert.NotNull(basedOfLectureMaterial.BaseLectureMaterialId);
        Assert.Equal(lectureMaterial.Id, basedOfLectureMaterial.BaseLectureMaterialId);
        Assert.Equal(basedOfAuthor.Name, basedOfLectureMaterial.Author.Name);
        Assert.Equal(basedOfAuthor.Id, basedOfLectureMaterial.Author.Id);
        Assert.Equal(basedLectureMaterialId, basedOfLectureMaterial.Id);

        Assert.Equal(name.Value, lectureMaterial.Name.Value);
        Assert.NotNull(lectureMaterial.Description.Title);
        Assert.NotNull(description.Title);
        Assert.Equal(description.Title.Value, lectureMaterial.Description.Title.Value);
        Assert.All(
            lectureMaterial.Description.Parts.Zip(description.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));
        Assert.NotNull(lectureMaterial.Data.Title);
        Assert.NotNull(data.Title);
        Assert.Equal(data.Title.Value, lectureMaterial.Data.Title.Value);
        Assert.All(
            lectureMaterial.Data.Parts.Zip(data.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));

        Assert.NotNull(lectureMaterialsRepository.FindEntity(lectureMaterialId));
        Assert.NotNull(lectureMaterialsRepository.FindEntity(basedLectureMaterialId));
    }

    [Fact]
    public void LectureMaterials_LectureMaterial_ChangeFields()
    {
        // Arrange
        var authorId = new Identifier();
        var lectureMaterialId = new Identifier();

        var author = new User(authorId, "Messi");
        var name = new TextUnit("OOP-OOP-OOP-OOP");
        Content description =
            Content.Builder
                .AddTitle(new TextUnit("Description"))
                .AddPart(new TextUnit("Do"))
                .AddPart(new TextUnit("Don't"))
                .Build();
        Content data =
            Content.Builder
                .AddTitle(new TextUnit("Content"))
                .AddPart(new TextUnit("Never gonna give you up"))
                .Build();

        var lectureMaterialsRepository = new InMemoryRepository<LectureMaterial>();
        var lectureMaterialsFactory = new LectureMaterialBuilderFactory(lectureMaterialsRepository);

        LectureMaterial lectureMaterial =
            lectureMaterialsFactory.CreateNewBuilder()
                .WithName(name)
                .WithDescription(description)
                .WithData(data)
                .WithAuthor(author)
                .WithIdentifier(lectureMaterialId)
                .Build();

        Content newDescription =
            Content.Builder
                .AddPart(new TextUnit("NEW DESCRIPTION"))
                .Build();
        Content newData =
            Content.Builder
                .AddPart(new TextUnit("NEW DATA"))
                .Build();

        var newName = new TextUnit("NEW NAME");
        var someUserId = new Identifier();
        var someUser = new User(someUserId, "Ronaldo");

        // Act
        ChangingFieldsResult changeNameResult = lectureMaterial.ChangeName(author, newName);
        ChangingFieldsResult changeDescriptionResult = lectureMaterial.ChangeDescription(author, newDescription);
        ChangingFieldsResult changeCriteriaResult1 = lectureMaterial.ChangeData(someUser, newData);
        ChangingFieldsResult changeCriteriaResult2 = lectureMaterial.ChangeData(author, newData);

        // Assert
        Assert.IsType<ChangingFieldsResult.Success>(changeNameResult);
        Assert.Equal(newName.Value, lectureMaterial.Name.Value);
        Assert.IsType<ChangingFieldsResult.Success>(changeDescriptionResult);
        Assert.Null(lectureMaterial.Description.Title);
        Assert.Null(newDescription.Title);
        Assert.All(
            lectureMaterial.Description.Parts.Zip(newDescription.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));
        Assert.IsType<ChangingFieldsResult.EditorIsNotTheAuthor>(changeCriteriaResult1);
        Assert.IsType<ChangingFieldsResult.Success>(changeCriteriaResult2);
        Assert.Null(lectureMaterial.Data.Title);
        Assert.Null(newData.Title);
        Assert.All(
            lectureMaterial.Data.Parts.Zip(newData.Parts),
            pair => Assert.Equal(pair.First.Value, pair.Second.Value));

        Assert.Null(lectureMaterial.BaseLectureMaterialId);
        Assert.Equal(lectureMaterialId, lectureMaterial.Id);
        Assert.Equal(author.Id, lectureMaterial.Author.Id);
        Assert.Equal(author.Name, lectureMaterial.Author.Name);

        Assert.Single(lectureMaterialsRepository.GetEntities());
        Assert.NotNull(lectureMaterialsRepository.FindEntity(lectureMaterialId));
    }

    [Fact]
    public void Subjects_Subject_NewBuild()
    {
        // Arrange
        var authorId = new Identifier();
        var subjectId = new Identifier();

        var name = new TextUnit("Chemistry");
        var author = new User(authorId, "Walter White");
        var examScore = new Score(25);

        var lab1AuthorId = new Identifier();
        var lab1Id = new Identifier();

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        LabWork labWork1 =
            labWorkFactory.CreateNewBuilder()
                .WithName(new TextUnit("Chapter 3"))
                .WithDescription(
                    Content.Builder
                        .AddTitle(new TextUnit("Description"))
                        .AddPart(new TextUnit("Do"))
                        .Build())
                .WithEvaluationCriteria(
                    Content.Builder
                        .AddTitle(new TextUnit("Criteria"))
                        .AddPart(new TextUnit("Stay quite"))
                        .Build())
                .WithWorth(new Score(50f))
                .WithAuthor(new User(lab1AuthorId, "Gustavo Fring"))
                .WithIdentifier(lab1Id)
                .Build();

        var lab2AuthorId = new Identifier();
        var lab2Id = new Identifier();

        LabWork labWork2 =
            labWorkFactory.CreateNewBuilder()
                .WithName(new TextUnit("Chapter 4"))
                .WithDescription(
                    Content.Builder
                        .AddPart(new TextUnit("Don't"))
                        .Build())
                .WithEvaluationCriteria(
                    Content.Builder
                        .AddPart(new TextUnit("Stay strong"))
                        .Build())
                .WithWorth(new Score(25f))
                .WithAuthor(new User(lab2AuthorId, "Skylar"))
                .WithIdentifier(lab2Id)
                .Build();

        var labWorks = new List<LabWork>([labWork1, labWork2]);

        var lectureMaterialAuthorId = new Identifier();
        var lectureMaterialId = new Identifier();

        var lectureMaterialsRepository = new InMemoryRepository<LectureMaterial>();
        var lectureMaterialsFactory = new LectureMaterialBuilderFactory(lectureMaterialsRepository);

        LectureMaterial lectureMaterial =
            lectureMaterialsFactory.CreateNewBuilder()
                .WithName(new TextUnit("Schedule"))
                .WithDescription(
                    Content.Builder
                        .AddTitle(new TextUnit("Morning"))
                        .AddPart(new TextUnit("Push Ups"))
                        .AddPart(new TextUnit("Running"))
                        .Build())
                .WithData(
                    Content.Builder
                        .AddTitle(new TextUnit("E"))
                        .Build())
                .WithAuthor(new User(lectureMaterialAuthorId, "Sam"))
                .WithIdentifier(lectureMaterialId)
                .Build();

        var lectureMaterials = new List<LectureMaterial>([lectureMaterial]);

        var subjectRepository = new InMemoryRepository<Subject>();
        var subjectsFactory = new SubjectBuilderFactory(subjectRepository);

        // Act
        Subject subject =
            subjectsFactory.CreateNewBuilder()
                .WithName(name)
                .WithExam(examScore)
                .WithLabWorks(labWorks)
                .WithLectureMaterials(lectureMaterials)
                .WithAuthor(author)
                .WithIdentifier(subjectId)
                .Build();

        // Assert
        Assert.Equal(subjectId, subject.Id);
        Assert.Equal(name, subject.Name);
        Assert.Equal(author, subject.Author);
        Assert.Equal(Format.Exam, subject.Format);
        Assert.Equal(examScore.Value, subject.Score.Value);
        Assert.Equal(2, subject.LabWorks.Count);
        Assert.Single(subject.LectureMaterials);

        Assert.Collection(
            subject.LabWorks,
            labWork =>
            {
                Assert.Equal("Chapter 3", labWork.Name.Value);
                Assert.NotNull(labWork.Description.Title);
                Assert.Equal("Description", labWork.Description.Title.Value);
                Assert.Collection(labWork.Description.Parts, part => Assert.Equal("Do", part.Value));
                Assert.NotNull(labWork.Criteria.Title);
                Assert.Equal("Criteria", labWork.Criteria.Title.Value);
                Assert.Collection(labWork.Criteria.Parts, part => Assert.Equal("Stay quite", part.Value));
                Assert.Equal(50f, labWork.Worth.Value);
                Assert.Equal("Gustavo Fring", labWork.Author.Name);
                Assert.Equal(lab1Id, labWork.Id);
            },
            labWork =>
            {
                Assert.Equal("Chapter 4", labWork.Name.Value);
                Assert.Null(labWork.Description.Title);
                Assert.Collection(labWork.Description.Parts, part => Assert.Equal("Don't", part.Value));
                Assert.Null(labWork.Criteria.Title);
                Assert.Collection(labWork.Criteria.Parts, part => Assert.Equal("Stay strong", part.Value));
                Assert.Equal(25f, labWork.Worth.Value);
                Assert.Equal("Skylar", labWork.Author.Name);
                Assert.Equal(lab2Id, labWork.Id);
            });

        Assert.Collection(
            subject.LectureMaterials,
            lecture =>
            {
                Assert.Equal("Schedule", lecture.Name.Value);
                Assert.NotNull(lecture.Description.Title);
                Assert.Equal("Morning", lecture.Description.Title.Value);
                Assert.Collection(
                    lecture.Description.Parts,
                    part1 => Assert.Equal("Push Ups", part1.Value),
                    part2 => Assert.Equal("Running", part2.Value));
                Assert.NotNull(lecture.Data.Title);
                Assert.Equal("E", lecture.Data.Title.Value);
                Assert.Equal("Sam", lecture.Author.Name);
                Assert.Equal(lectureMaterialId, lecture.Id);
            });

        Assert.Equal(labWork1, labWorksRepository.GetEntity(lab1Id));
        Assert.Equal(labWork2, labWorksRepository.GetEntity(lab2Id));
        Assert.Equal(lectureMaterial, lectureMaterialsRepository.GetEntity(lectureMaterialId));
        Assert.Equal(subject, subjectRepository.GetEntity(subjectId));
    }

    [Fact]
    public void Subjects_Subject_BuildFail()
    {
        // Arrange
        var authorId = new Identifier();
        var subjectId = new Identifier();

        var name = new TextUnit("Chemistry");
        var author = new User(authorId, "Walter White");
        var examScore = new Score(35);

        var lab1AuthorId = new Identifier();
        var lab1Id = new Identifier();

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        LabWork labWork1 =
            labWorkFactory.CreateNewBuilder()
                .WithName(new TextUnit("Chapter 3"))
                .WithDescription(
                    Content.Builder
                        .AddTitle(new TextUnit("Description"))
                        .AddPart(new TextUnit("Do"))
                        .Build())
                .WithEvaluationCriteria(
                    Content.Builder
                        .AddTitle(new TextUnit("Criteria"))
                        .AddPart(new TextUnit("Stay quite"))
                        .Build())
                .WithWorth(new Score(50f))
                .WithAuthor(new User(lab1AuthorId, "Gustavo Fring"))
                .WithIdentifier(lab1Id)
                .Build();

        var lab2AuthorId = new Identifier();
        var lab2Id = new Identifier();

        LabWork labWork2 =
            labWorkFactory.CreateNewBuilder()
                .WithName(new TextUnit("Chapter 4"))
                .WithDescription(
                    Content.Builder
                        .AddPart(new TextUnit("Don't"))
                        .Build())
                .WithEvaluationCriteria(
                    Content.Builder
                        .AddPart(new TextUnit("Stay strong"))
                        .Build())
                .WithWorth(new Score(25f))
                .WithAuthor(new User(lab2AuthorId, "Skylar"))
                .WithIdentifier(lab2Id)
                .Build();

        var labWorks = new List<LabWork>([labWork1, labWork2]);

        var lectureMaterialAuthorId = new Identifier();
        var lectureMaterialId = new Identifier();

        var lectureMaterialsRepository = new InMemoryRepository<LectureMaterial>();
        var lectureMaterialsFactory = new LectureMaterialBuilderFactory(lectureMaterialsRepository);

        LectureMaterial lectureMaterial =
            lectureMaterialsFactory.CreateNewBuilder()
                .WithName(new TextUnit("Schedule"))
                .WithDescription(
                    Content.Builder
                        .AddTitle(new TextUnit("Morning"))
                        .AddPart(new TextUnit("Push Ups"))
                        .AddPart(new TextUnit("Running"))
                        .Build())
                .WithData(
                    Content.Builder
                        .AddTitle(new TextUnit("E"))
                        .Build())
                .WithAuthor(new User(lectureMaterialAuthorId, "Sam"))
                .WithIdentifier(lectureMaterialId)
                .Build();

        var lectureMaterials = new List<LectureMaterial>([lectureMaterial]);

        var subjectRepository = new InMemoryRepository<Subject>();
        var subjectsFactory = new SubjectBuilderFactory(subjectRepository);

        // Act
        Exception? exception = Record.Exception(() =>
            subjectsFactory.CreateNewBuilder()
                .WithName(name)
                .WithExam(examScore)
                .WithLabWorks(labWorks)
                .WithLectureMaterials(lectureMaterials)
                .WithAuthor(author)
                .WithIdentifier(subjectId)
                .Build());

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Subjects_Subject_BasedOfBuild()
    {
        // Arrange
        var authorId = new Identifier();
        var subjectId = new Identifier();

        var author = new User(authorId, "Author1");
        var name = new TextUnit("Subj001");
        var examScore = new Score(25);

        var lab1AuthorId = new Identifier();
        var lab1Id = new Identifier();

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        LabWork labWork1 =
            labWorkFactory.CreateNewBuilder()
                .WithName(new TextUnit("Chap001"))
                .WithDescription(
                    Content.Builder
                        .AddTitle(new TextUnit("Desc001"))
                        .AddPart(new TextUnit("Part001"))
                        .Build())
                .WithEvaluationCriteria(
                    Content.Builder
                        .AddTitle(new TextUnit("Crit001"))
                        .AddPart(new TextUnit("Part002"))
                        .Build())
                .WithWorth(new Score(75f))
                .WithAuthor(new User(lab1AuthorId, "Auth002"))
                .WithIdentifier(lab1Id)
                .Build();

        var labWorks = new List<LabWork>([labWork1]);

        var lectureMaterialAuthorId = new Identifier();
        var lectureMaterialId = new Identifier();

        var lectureMaterialsRepository = new InMemoryRepository<LectureMaterial>();
        var lectureMaterialsFactory = new LectureMaterialBuilderFactory(lectureMaterialsRepository);

        LectureMaterial lectureMaterial =
            lectureMaterialsFactory.CreateNewBuilder()
                .WithName(new TextUnit("Sched01"))
                .WithDescription(
                    Content.Builder
                        .AddTitle(new TextUnit("Morn001"))
                        .AddPart(new TextUnit("Exer001"))
                        .AddPart(new TextUnit("Run0012"))
                        .Build())
                .WithData(
                    Content.Builder
                        .AddTitle(new TextUnit("Cont001"))
                        .Build())
                .WithAuthor(new User(lectureMaterialAuthorId, "Auth003"))
                .WithIdentifier(lectureMaterialId)
                .Build();

        var lectureMaterials = new List<LectureMaterial>([lectureMaterial]);

        var subjectRepository = new InMemoryRepository<Subject>();
        var subjectsFactory = new SubjectBuilderFactory(subjectRepository);

        Subject originalSubject =
            subjectsFactory.CreateNewBuilder()
                .WithName(name)
                .WithExam(examScore)
                .WithLabWorks(labWorks)
                .WithLectureMaterials(lectureMaterials)
                .WithAuthor(author)
                .WithIdentifier(subjectId)
                .Build();

        var newAuthorId = new Identifier();
        var newAuthor = new User(newAuthorId, "Auth004");
        var newSubjectId = new Identifier();

        // Act
        Subject basedOfSubject =
            subjectsFactory.CreateBasedOfBuilder()
                .BasedOf(originalSubject, labWorksRepository, lectureMaterialsRepository)
                .WithAuthor(newAuthor)
                .WithIdentifier(newSubjectId)
                .Build();

        // Assert
        Assert.Collection(
            basedOfSubject.LabWorks,
            labWork =>
            {
                Assert.Equal("Chap001", labWork.Name.Value);
                Assert.NotNull(labWork.Description.Title);
                Assert.Equal("Desc001", labWork.Description.Title.Value);
                Assert.Collection(labWork.Description.Parts, part => Assert.Equal("Part001", part.Value));
                Assert.NotNull(labWork.Criteria.Title);
                Assert.Equal("Crit001", labWork.Criteria.Title.Value);
                Assert.Collection(labWork.Criteria.Parts, part => Assert.Equal("Part002", part.Value));
                Assert.Equal(75f, labWork.Worth.Value);
                Assert.Equal("Auth002", labWork.Author.Name);
                Assert.NotEqual(lab1Id, labWork.Id);
            });

        Assert.Collection(
            basedOfSubject.LectureMaterials,
            lecture =>
            {
                Assert.Equal("Sched01", lecture.Name.Value);
                Assert.NotNull(lecture.Description.Title);
                Assert.Equal("Morn001", lecture.Description.Title.Value);
                Assert.Collection(
                    lecture.Description.Parts,
                    part1 => Assert.Equal("Exer001", part1.Value),
                    part2 => Assert.Equal("Run0012", part2.Value));
                Assert.NotNull(lecture.Data.Title);
                Assert.Equal("Cont001", lecture.Data.Title.Value);
                Assert.Equal("Auth003", lecture.Author.Name);
                Assert.NotEqual(lectureMaterialId, lecture.Id);
            });

        Assert.Equal(2, labWorksRepository.GetEntities().Count());
        Assert.Equal(2, lectureMaterialsRepository.GetEntities().Count());
        Assert.Equal(2, subjectRepository.GetEntities().Count());
    }

    [Fact]
    public void Subjects_Subject_ChangeFields()
    {
        // Arrange
        var authorId = new Identifier();
        var subjectId = new Identifier();

        var author = new User(authorId, "Auth005");
        var name = new TextUnit("Subj002");
        var passScore = new Score(60);

        var lab1AuthorId = new Identifier();
        var lab1Id = new Identifier();

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        LabWork labWork1 =
            labWorkFactory.CreateNewBuilder()
                .WithName(new TextUnit("Chap002"))
                .WithDescription(
                    Content.Builder
                        .AddTitle(new TextUnit("Desc002"))
                        .AddPart(new TextUnit("Part003"))
                        .Build())
                .WithEvaluationCriteria(
                    Content.Builder
                        .AddTitle(new TextUnit("Crit002"))
                        .AddPart(new TextUnit("Part004"))
                        .Build())
                .WithWorth(new Score(100f))
                .WithAuthor(new User(lab1AuthorId, "Auth006"))
                .WithIdentifier(lab1Id)
                .Build();

        var labWorksRepo = new List<LabWork>([labWork1]);

        var lectureMaterialAuthorId = new Identifier();
        var lectureMaterialId = new Identifier();

        var lectureMaterialsRepository = new InMemoryRepository<LectureMaterial>();
        var lectureMaterialsFactory = new LectureMaterialBuilderFactory(lectureMaterialsRepository);

        LectureMaterial lectureMaterial =
            lectureMaterialsFactory.CreateNewBuilder()
                .WithName(new TextUnit("Sched02"))
                .WithDescription(
                    Content.Builder
                        .AddTitle(new TextUnit("Morn002"))
                        .AddPart(new TextUnit("Exer002"))
                        .Build())
                .WithData(
                    Content.Builder
                        .AddTitle(new TextUnit("Cont002"))
                        .Build())
                .WithAuthor(new User(lectureMaterialAuthorId, "Auth007"))
                .WithIdentifier(lectureMaterialId)
                .Build();

        var lectureMaterialsRepo = new List<LectureMaterial>([lectureMaterial]);

        var subjectRepository = new InMemoryRepository<Subject>();
        var subjectsFactory = new SubjectBuilderFactory(subjectRepository);

        Subject subject =
            subjectsFactory.CreateNewBuilder()
                .WithName(name)
                .WithPass(passScore)
                .WithLabWorks(labWorksRepo)
                .WithLectureMaterials(lectureMaterialsRepo)
                .WithAuthor(author)
                .WithIdentifier(subjectId)
                .Build();

        var newName = new TextUnit("Subj003");
        var newScore = new Score(70);

        // Act
        ChangingFieldsResult nameChangeResult = subject.ChangeName(author, newName);
        ChangingFieldsResult scoreChangeResult = subject.ChangePassScore(author, newScore);

        // Assert
        Assert.IsType<ChangingFieldsResult.Success>(nameChangeResult);
        Assert.Equal(newName.Value, subject.Name.Value);

        Assert.IsType<ChangingFieldsResult.Success>(scoreChangeResult);
        Assert.Equal(70, subject.Score.Value);

        Assert.Collection(
            subject.LabWorks,
            labWork =>
            {
                Assert.Equal("Chap002", labWork.Name.Value);
                Assert.Equal("Auth006", labWork.Author.Name);
            });
    }

    [Fact]
    public void EducationalPrograms_EducationalProgram_Build()
    {
        // Arrange
        var directorId = new Identifier();
        var educationalProgramId = new Identifier();

        var name = new TextUnit("ProgramX");
        var director = new User(directorId, "Dr. John Doe");
        var semester = new Semester(1);
        var examScore = new Score(50);

        var lab1AuthorId = new Identifier();
        var lab1Id = new Identifier();

        var labWorksRepository = new InMemoryRepository<LabWork>();
        var labWorkFactory = new LabWorkBuilderFactory(labWorksRepository);

        LabWork labWork1 =
            labWorkFactory.CreateNewBuilder()
                .WithName(new TextUnit("Chapter 3"))
                .WithDescription(Content.Builder
                    .AddTitle(new TextUnit("Description"))
                    .AddPart(new TextUnit("Do"))
                    .Build())
                .WithEvaluationCriteria(Content.Builder
                    .AddTitle(new TextUnit("Criteria"))
                    .AddPart(new TextUnit("Stay quite"))
                    .Build())
                .WithWorth(new Score(100f))
                .WithAuthor(new User(lab1AuthorId, "Gustavo Fring"))
                .WithIdentifier(lab1Id)
                .Build();

        var labWorks = new List<LabWork>([labWork1]);

        var lectureMaterialAuthorId = new Identifier();
        var lectureMaterialId = new Identifier();

        var lectureMaterialsRepository = new InMemoryRepository<LectureMaterial>();
        var lectureMaterialsFactory = new LectureMaterialBuilderFactory(lectureMaterialsRepository);

        LectureMaterial lectureMaterial =
            lectureMaterialsFactory.CreateNewBuilder()
                .WithName(new TextUnit("Schedule"))
                .WithDescription(Content.Builder
                    .AddTitle(new TextUnit("Morning"))
                    .AddPart(new TextUnit("Push Ups"))
                    .Build())
                .WithData(Content.Builder
                    .AddTitle(new TextUnit("E"))
                    .Build())
                .WithAuthor(new User(lectureMaterialAuthorId, "Sam"))
                .WithIdentifier(lectureMaterialId)
                .Build();

        var lectureMaterials = new List<LectureMaterial>([lectureMaterial]);

        var subjectId = new Identifier();

        var subjectRepository = new InMemoryRepository<Subject>();
        var subjectsFactory = new SubjectBuilderFactory(subjectRepository);

        Subject subject = CreateSubject("Subj001", 60, new User(directorId, "Dr."), labWorks, lectureMaterials);

        var educationalProgramRepository = new InMemoryRepository<EducationalProgram>();
        var educationalProgramFactory = new EducationalProgramBuilderFactory(educationalProgramRepository);

        // Act
        EducationalProgram educationalProgram =
            educationalProgramFactory.CreateBuilder()
                .WithName(name)
                .WithDirector(director)
                .WithIdentifier(educationalProgramId)
                .AddSubjectToSemester(semester, subject)
                .Build();

        // Assert
        Assert.Equal(name.Value, educationalProgram.Name.Value);
        Assert.Equal(educationalProgramId, educationalProgram.Id);
        Assert.Equal(director.Id, educationalProgram.Director.Id);
        Assert.Single(educationalProgram.GetSubjectsInSemester(semester));
    }

    private Subject CreateSubject(
        string name,
        int score,
        User author,
        IReadOnlyCollection<LabWork> labWorks,
        IReadOnlyCollection<LectureMaterial> lectureMaterials)
    {
        var subjectsFactory = new SubjectBuilderFactory(new InMemoryRepository<Subject>());
        return subjectsFactory.CreateNewBuilder()
            .WithName(new TextUnit(name))
            .WithPass(new Score(score))
            .WithLabWorks(labWorks)
            .WithLectureMaterials(lectureMaterials)
            .WithAuthor(author)
            .WithIdentifier(new Identifier())
            .Build();
    }
}
