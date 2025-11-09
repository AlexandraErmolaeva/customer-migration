using Domain.Entities.General.Interfaces;

namespace Domain.Entities.General;

public abstract class EntityBase : IEntityBase, IAuditable
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
}
