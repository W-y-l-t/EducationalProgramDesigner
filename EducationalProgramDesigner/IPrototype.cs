namespace EducationalProgramDesigner;

public interface IPrototype<T> where T : IPrototype<T>
{
    T Clone();
}
