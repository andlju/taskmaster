using System;

namespace Taskmaster.Service.Events
{
    public class UserAddedEvent : IEvent
    {
        public readonly Guid UserAggregateId;
        public readonly string Name;
        public readonly Guid CreatedByUserId;

        public UserAddedEvent(Guid userAggregateId, string name, Guid createdByUserId)
        {
            UserAggregateId = userAggregateId;
            Name = name;
            CreatedByUserId = createdByUserId;
        }

        public Guid AggregateId
        {
            get { return UserAggregateId; }
        }
    }
}