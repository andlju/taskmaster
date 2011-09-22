using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.EventHandlers;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.CommandHandlers.Tests
{
    [TestClass]
    public abstract class WithCommandHandlerFor<T>
    {
        class SimpleEventStorage : IEventStorage
        {
            public List<object> StoredEvents = new List<object>();

            public void Store<TEvent>(TEvent evt) where TEvent : IEvent
            {
                StoredEvents.Add(evt);
            }

            public IEnumerable<IEvent> GetEventsSince(DateTime minValue)
            {
                throw new NotImplementedException();
            }
        }

        protected abstract ICommandHandler<T> Given(IEventStorage eventStorage);
        protected abstract T When();

        protected Guid _authenticatedUserId = Guid.NewGuid();
        private SimpleEventStorage _eventStorage = new SimpleEventStorage();

        protected Exception ThrownException { get; private set; }
        protected object[] Events { get { return _eventStorage.StoredEvents.ToArray(); } }

        protected TEvent Event<TEvent>(int index)
        {
            return (TEvent)Events[index];
        }

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                var handler = Given(_eventStorage);
                var command = When();

                handler.Handle(command);
            }
            catch (Exception ex)
            {
                ThrownException = ex;
            }
        }
    }
}