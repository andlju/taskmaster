﻿using System;

namespace Taskmaster.Service.Events
{
    public class TaskItemAddedEvent : IEvent
    {
        public readonly Guid TaskItemAggregateId;
        public readonly string Title;
        public readonly string Details;
        public readonly Guid? AssignedUserAggregateId;
        public readonly Guid CreatedByUserAggregateId;

        public TaskItemAddedEvent(Guid taskItemAggregateId, string title, string details, Guid? assignedUserAggregateId, Guid createdByUserAggregateId)
        {
            TaskItemAggregateId = taskItemAggregateId;
            Title = title;
            Details = details;
            AssignedUserAggregateId = assignedUserAggregateId;
            CreatedByUserAggregateId = createdByUserAggregateId;
        }

        public Guid AggregateId
        {
            get { return TaskItemAggregateId; }
        }
    }
}