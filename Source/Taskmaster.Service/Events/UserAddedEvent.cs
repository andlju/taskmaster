using System;

namespace Taskmaster.Service.Events
{
    public class UserAddedEvent
    {
        public readonly Guid UserAggregateId;
        public readonly string Name;

        public UserAddedEvent(Guid userAggregateId, string name)
        {
            UserAggregateId = userAggregateId;
            Name = name;
        }
    }
}