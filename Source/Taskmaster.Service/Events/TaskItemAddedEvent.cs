using System;

namespace Taskmaster.Service.Events
{
    public class TaskItemAddedEvent
    {
        public readonly Guid TaskItemAggregateId;
        public readonly string Title;
        public readonly string Details;
        public readonly Guid? AssignedUserAggregateId;

        public TaskItemAddedEvent(Guid taskItemAggregateId, string title, string details, Guid? assignedUserAggregateId)
        {
            TaskItemAggregateId = taskItemAggregateId;
            Title = title;
            Details = details;
            AssignedUserAggregateId = assignedUserAggregateId;
        }
    }
}