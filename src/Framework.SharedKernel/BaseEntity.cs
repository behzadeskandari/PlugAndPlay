namespace Framework.SharedKernel;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }

    public void MarkDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkUpdated()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
