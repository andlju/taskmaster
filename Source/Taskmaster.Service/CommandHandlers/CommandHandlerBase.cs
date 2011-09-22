using System;
using Taskmaster.Domain;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service.CommandHandlers
{
    public class CommandHandlerBase
    {
        private readonly IEventStorage _storage;

        public CommandHandlerBase(IEventStorage storage)
        {
            _storage = storage;
        }

        protected void Store<T>(T evt) where T : IEvent
        {
            _storage.Store(evt);
        }

    }
}