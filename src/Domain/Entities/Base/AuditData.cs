namespace Crawler.Domain.Entities.Base;

public class AuditData : IEntity
{
    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }
}
