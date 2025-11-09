namespace Domain.Entities.General.Interfaces;

public interface IAuditable : ISoftDeletable
{
    public DateTime CreatedAt { get; }
    public DateTime? LastModifiedAt { get; }
}
