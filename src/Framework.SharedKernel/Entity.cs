namespace Framework.SharedKernel;

public abstract class Entity<TId> : BaseEntity
    where TId : notnull
{
    public TId IdValue { get; protected set; } = default!;

    protected Entity(TId idValue)
    {
        IdValue = idValue;
    }
}
