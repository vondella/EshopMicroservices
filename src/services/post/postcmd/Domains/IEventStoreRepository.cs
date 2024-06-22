using postcmd.posts.Events;

namespace postcmd.Domains
{
    public interface IEventStoreRepository
    {
        Task SaveAsync(EventModel @event);
        Task<List<EventModel>> FindByAggregate(Guid aggregateId);
    }
}
