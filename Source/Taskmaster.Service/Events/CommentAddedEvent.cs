using System;

namespace Taskmaster.Service.Events
{
    public class CommentAddedEvent : IEvent
    {
        public readonly Guid TaskItemAggregateId;
        public readonly Guid CommentId;
        public readonly string Comment;
        public readonly Guid CreatedByUserId;

        public CommentAddedEvent(Guid taskItemAggregateId, Guid commentId, string comment, Guid createdByUserId)
        {
            TaskItemAggregateId = taskItemAggregateId;
            CommentId = commentId;
            Comment = comment;
            CreatedByUserId = createdByUserId;
        }

        public Guid AggregateId
        {
            get { return TaskItemAggregateId; }
        }
    }
}