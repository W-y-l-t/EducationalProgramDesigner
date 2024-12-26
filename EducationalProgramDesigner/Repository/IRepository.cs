using EducationalProgramDesigner.Entities;
using EducationalProgramDesigner.ValueObjects;

namespace EducationalProgramDesigner.Repository;

public interface IRepository<T> where T : class, IIdentifier
{
    void AddEntity(T entity);

    T? FindEntity(Identifier entityId);

    T GetEntity(Identifier entityId);

    void RemoveEntity(Identifier entityId);

    void RemoveEntity(T entity);

    IEnumerable<T> GetEntities();
}
