using System;

namespace Taskmaster.Service.Commands
{
    public class EditTaskItemCommand
    {
        public readonly Guid TaskItemAggregateId;
        public readonly string Title;
        public readonly string Details;
        public readonly Guid? AssignedUserAggregateId;

        public EditTaskItemCommand(Guid taskItemAggregateId, string title, string details, Guid? assignedUserAggregateId)
        {
            TaskItemAggregateId = taskItemAggregateId;
            Title = title;
            Details = details;
            AssignedUserAggregateId = assignedUserAggregateId;
        }
    }
}