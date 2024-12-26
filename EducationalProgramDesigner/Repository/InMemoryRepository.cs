using EducationalProgramDesigner.Entities;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Repository;

public class InMemoryRepository<T> : IRepository<T> where T : class, IIdentifier
{
    private readonly List<T> _items;

    public InMemoryRepository()
    {
        _items = [];
    }

    public InMemoryRepository(IEnumerable<T> items)
    {
        _items = [.. items];
    }

    public void AddEntity(T entity)
    {
        _items.Add(entity);
    }

    public T? FindEntity(Identifier entityId)
    {
        return _items.FirstOrDefault(x => x.Id == entityId);
    }

    public T GetEntity(Identifier entityId)
    {
        return _items.First(x => x.Id == entityId);
    }

    public void RemoveEntity(Identifier entityId)
    {
        _items.RemoveAll(x => x.Id == entityId);
    }

    public void RemoveEntity(T entity)
    {
        _items.Remove(entity);
    }

    public IEnumerable<T> GetEntities()
    {
        return _items;
    }
}
