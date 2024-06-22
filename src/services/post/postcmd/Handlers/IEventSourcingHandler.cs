namespace postcmd.Handlers
{
    public interface IEventSourcingHandler<T>
    {
        Task SaveAsync(AggregateRoot aggregateRoot);
        Task<T> GetById(Guid Id);
    }
}
