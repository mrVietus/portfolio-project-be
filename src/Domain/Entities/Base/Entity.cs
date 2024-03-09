namespace Crawler.Domain.Entities.Base;

public abstract class Entity
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public DateTime? Updated { get; private set; }

    protected Entity(Guid id, DateTime creationDate)
    {
        Id = id;
        Created = creationDate;
    }

    protected Entity() { }
}
