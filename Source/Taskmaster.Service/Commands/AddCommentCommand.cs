using System;

namespace Taskmaster.Service.Commands
{
    public class Command
    {
        public readonly Guid CommandId;
        public readonly Guid AuthenticatedUserId;

        public Command(Guid commandId, Guid authenticatedUserId)
        {
            CommandId = commandId;
            AuthenticatedUserId = authenticatedUserId;
        }

        public Command(Guid authenticatedUserId)
        {
            AuthenticatedUserId = authenticatedUserId;
        }
    }

    public class AddCommentCommand : Command
    {
        public readonly Guid TaskItemAggregateId;
        public readonly Guid CommentId;
        public readonly string Comment;

        public AddCommentCommand(Guid authenticatedUserId, Guid taskItemAggregateId, Guid commentId, string comment) : 
            base(authenticatedUserId)
        {
            TaskItemAggregateId = taskItemAggregateId;
            CommentId = commentId;
            Comment = comment;
        }
    }
}