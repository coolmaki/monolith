namespace Monolith.Lib.Domain;

public abstract class AggregateRoot : IEntity
{
    // ------------------------------------------------------------
    // Constructors & Factories
    // ------------------------------------------------------------

    protected AggregateRoot()
    {
    }

    // ------------------------------------------------------------
    // Backing Fields
    // ------------------------------------------------------------

    private readonly List<IDomainEvent> _domainEvents = [];

    // ------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // ------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}