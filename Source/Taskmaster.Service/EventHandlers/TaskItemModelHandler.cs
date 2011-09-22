using System;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Events;

namespace Taskmaster.Service.EventHandlers
{
    public class TaskItemModelHandler : 
        IEventHandler<TaskItemAddedEvent>,
        IEventHandler<TaskItemEditedEvent>,
        IEventHandler<CommentAddedEvent>        
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IIdentityLookup _identityLookup;
        private readonly IObjectContext _objectContext;

        public TaskItemModelHandler(ITaskItemRepository taskItemRepository, IIdentityLookup identityLookup, IObjectContext objectContext)
        {
            _taskItemRepository = taskItemRepository;
            _identityLookup = identityLookup;
            _objectContext = objectContext;
        }

        public void Handle(TaskItemAddedEvent evt)
        {
            int createdByUserModelId = GetUserModelId(evt.CreatedByUserAggregateId).GetValueOrDefault();
            int? assignedToUserModelId = GetUserModelId(evt.AssignedUserAggregateId);

            var taskItem = new Domain.TaskItem()
            {
                CreatedByUserId = createdByUserModelId,
                Title = evt.Title,
                Details = evt.Details,
                AssignedToUserId = assignedToUserModelId,
            };
            _taskItemRepository.Add(taskItem);

            _objectContext.SaveChanges();

            _identityLookup.StoreMapping<Domain.TaskItem>(evt.TaskItemAggregateId, taskItem.TaskItemId);
        }

        public void Handle(TaskItemEditedEvent evt)
        {
            var taskItemIdModel = _identityLookup.GetModelId<Domain.TaskItem>(evt.TaskItemAggregateId);

            int? assignedToUserModelId = GetUserModelId(evt.AssignedUserAggregateId);

            var taskItem = _taskItemRepository.Get(t => t.TaskItemId == taskItemIdModel);

            taskItem.Title = evt.Title;
            taskItem.Details = evt.Details;
            taskItem.AssignedToUserId = assignedToUserModelId;

            _objectContext.SaveChanges();
        }

        public void Handle(CommentAddedEvent evt)
        {
            var taskModelId = _identityLookup.GetModelId<Domain.TaskItem>(evt.TaskItemAggregateId);

            var createdByModelid = GetUserModelId(evt.CreatedByUserId).Value;

            var taskItem = _taskItemRepository.Get(t => t.TaskItemId == taskModelId);

            var comment = new Domain.TaskComment()
            {
                Comment = evt.Comment,
                CreatedByUserId = createdByModelid
            };
            taskItem.Comments.Add(comment);

            _objectContext.SaveChanges();

            _identityLookup.StoreMapping<Domain.TaskComment>(evt.CommentId, comment.TaskCommentId);
        }

        protected int? GetUserModelId(Guid? userAggregateId)
        {
            int? userModelId = null;
            if (userAggregateId != null)
            {
                userModelId = _identityLookup.GetModelId<Domain.User>(userAggregateId.Value);
            }
            return userModelId;
        }

    }
}