using System;

namespace Taskmaster.Service.Events
{
    public interface IEvent
    {
        Guid AggregateId { get; } 
    }
}