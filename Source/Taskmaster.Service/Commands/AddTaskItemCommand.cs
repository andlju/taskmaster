using System;

namespace Taskmaster.Service.Commands
{
    public class AddTaskItemCommand
    {
        public readonly Guid TaskItemAggregateId;
        public readonly string Title;
        public readonly string Details;
        public readonly Guid? AssignedUserAggregateId;

        public AddTaskItemCommand(Guid taskItemAggregateId, string title, string details, Guid? assignedUserAggregateId)
        {
            TaskItemAggregateId = taskItemAggregateId;
            Title = title;
            Details = details;
            AssignedUserAggregateId = assignedUserAggregateId;
        }
    }
}