using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taskmaster.Service.CommandHandlers;

namespace Taskmaster.CommandHandlers.Tests
{
    [TestClass]
    public abstract class WithCommandHandlerFor<T>
    {
        protected abstract ICommandHandler<T> Given();
        protected abstract T When();

        protected Guid _authenticatedUserId = Guid.NewGuid();


        protected Exception ThrownException { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                var handler = Given();
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