using System;

namespace Taskmaster.Service.Events
{
    public class TaskItemEditedEvent
    {
        public readonly Guid TaskItemAggregateId;
        public readonly string Title;
        public readonly string Details;
        public readonly Guid? AssignedUserAggregateId;

        public TaskItemEditedEvent(Guid taskItemAggregateId, string title, string details, Guid? assignedUserAggregateId)
        {
            TaskItemAggregateId = taskItemAggregateId;
            Title = title;
            Details = details;
            AssignedUserAggregateId = assignedUserAggregateId;
        }
    }
}