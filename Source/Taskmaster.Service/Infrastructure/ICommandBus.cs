using System;
using System.Collections.Generic;
using Taskmaster.Service.CommandHandlers;

namespace Taskmaster.Service.Infrastructure
{
    public interface ICommandBus
    {
        void Publish<T>(T command);
        void RegisterHandler<T>(ICommandHandler<T> handler);
    }

    public class CommandBus : ICommandBus
    {
        private readonly Dictionary<Type, object> _handlers = new Dictionary<Type, object>();

        public void Publish<T>(T command)
        {
            var type = typeof (T);
            var handler = (ICommandHandler<T>)_handlers[type];
            
            handler.Handle(command);
        }

        public void RegisterHandler<T>(ICommandHandler<T> handler)
        {
            var type = typeof (T);
            _handlers[type] = handler;
        }
    }
}