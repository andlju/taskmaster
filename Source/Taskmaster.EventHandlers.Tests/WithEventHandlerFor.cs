using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taskmaster.Service.EventHandlers;
using Taskmaster.Service.Events;

namespace Taskmaster.EventHandlers.Tests
{
    [TestClass]
    public abstract class WithEventHandlerFor<TEvent>
    {
        protected abstract IEventHandler<TEvent> Given();
        protected abstract TEvent When();

        protected Exception ThrownException { get; private set; }

        protected Guid _createdByUserId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                var handler = Given();
                var evt = When();

                handler.Handle(evt);
            }
            catch (Exception ex)
            {
                ThrownException = ex;
            }
        }
    }
}