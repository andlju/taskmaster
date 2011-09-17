using System;

namespace Taskmaster.Service.Commands
{
    public class EditTaskItemCommand : Command
    {
        public readonly Guid TaskItemAggregateId;
        public readonly string Title;
        public readonly string Details;
        public readonly Guid? AssignedUserAggregateId;

        public EditTaskItemCommand(Guid authenticatedUserId, Guid taskItemAggregateId, string title, string details, Guid? assignedUserAggregateId) : base(authenticatedUserId)
        {
            TaskItemAggregateId = taskItemAggregateId;
            Title = title;
            Details = details;
            AssignedUserAggregateId = assignedUserAggregateId;
        }
    }
}