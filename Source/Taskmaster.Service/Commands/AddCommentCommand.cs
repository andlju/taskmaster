using System;

namespace Taskmaster.Service.Commands
{
    public class AddCommentCommand
    {
        public readonly Guid TaskItemAggregateId;
        public readonly Guid CommentId;
        public readonly string Comment;

        public AddCommentCommand(Guid taskItemAggregateId, Guid commentId, string comment)
        {
            TaskItemAggregateId = taskItemAggregateId;
            CommentId = commentId;
            Comment = comment;
        }
    }
}