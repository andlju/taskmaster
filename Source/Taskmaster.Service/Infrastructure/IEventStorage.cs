using System;
using EventStore;
using EventStore.Dispatcher;
using Taskmaster.Service.Events;

namespace Taskmaster.Service.Infrastructure
{
    public interface IEventStorage
    {
        void Store<T>(T evt) where T : IEvent;
    }

    public class EventStoreStorage : IEventStorage
    {
        private readonly IStoreEvents _store;
        private readonly IEventBus _eventBus;

        public EventStoreStorage(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _store = Wireup.Init().UsingSqlPersistence("TaskMasterEventStore").
                UsingJsonSerialization().
                UsingSynchronousDispatcher(
                    new DelegateMessagePublisher(c =>
                                                     {
                                                         foreach (var e in c.Events)
                                                         {
                                                             _eventBus.Publish(e.Body);
                                                         }
                                                     })
                ).Build();
        }

        public void Store<T>(T evt) 
            where T : IEvent
        {
            using (var stream = _store.OpenStream(evt.AggregateId, 0, int.MaxValue))
            {
                stream.Add(new EventMessage() { Body = evt });
                
                stream.CommitChanges(Guid.NewGuid());
            }
        }
    }
}