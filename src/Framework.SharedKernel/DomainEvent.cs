namespace Framework.SharedKernel;

public interface IDomainEvent
{
}

public abstract record DomainEvent : IDomainEvent;
