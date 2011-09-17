using System;

namespace Taskmaster.Service.Events
{
    public class CommentAddedEvent
    {
        public readonly Guid TaskItemAggregateId;
        public readonly Guid CommentId;
        public readonly string Comment;

        public CommentAddedEvent(Guid taskItemAggregateId, Guid commentId, string comment)
        {
            TaskItemAggregateId = taskItemAggregateId;
            CommentId = commentId;
            Comment = comment;
        }
    }
}