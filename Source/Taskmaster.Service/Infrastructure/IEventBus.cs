using System;
using System.Collections.Generic;
using Taskmaster.Service.EventHandlers;

namespace Taskmaster.Service.Infrastructure
{
    public interface IEventBus
    {
        void Publish(object evt);
        void RegisterHandler<T>(IEventHandler<T> handler);
    }

    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Action<object>>> _eventHandlers = new Dictionary<Type, List<Action<object>>>();

        public void Publish(object evt)
        {
            List<Action<object>> eventHandlerList;
            if (_eventHandlers.TryGetValue(evt.GetType(), out eventHandlerList))
            {
                foreach(var handler in eventHandlerList)
                {
                    handler(evt);
                }
            }
        }

        public void RegisterHandler<T>(IEventHandler<T> handler)
        {
            List<Action<object>> eventHandlerList;
            if (!_eventHandlers.TryGetValue(typeof(T), out eventHandlerList))
            {
                eventHandlerList = new List<Action<object>>();
                _eventHandlers[typeof(T)] = eventHandlerList;
            }
            eventHandlerList.Add(e => handler.Handle((T)e));
        }
    }
}