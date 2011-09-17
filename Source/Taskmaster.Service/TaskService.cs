using System;
using System.Collections.Generic;
using AutoMapper;
using Taskmaster.Domain;
using Taskmaster.Service.Bus;
using Taskmaster.Service.Commands;

namespace Taskmaster.Service
{
    public class TaskService : ITaskService
    {
        public const int StatusOk = 0;
        public const int StatusNotFound = 1;

        private readonly ITaskItemRepository _taskItemRepository;

        private readonly ICommandBus _commandBus;
        private readonly IIdentityLookup _identityLookup;

        public TaskService(ITaskItemRepository taskItemRepository, ICommandBus commandBus, IIdentityLookup identityLookup)
        {
            _taskItemRepository = taskItemRepository;
            _commandBus = commandBus;
            _identityLookup = identityLookup;

            Mapper.CreateMap<Domain.TaskItem, TaskItem>();
            Mapper.CreateMap<Domain.TaskComment, TaskComment>();
        }

        public AddTaskItemResponse AddTaskItem(AddTaskItemRequest request)
        {
            var taskItemAggregateId = Guid.NewGuid();

            Guid? assignedUserAggregateId = GetUserAggregateId(request.AssignedToUserId);

            _commandBus.Publish(new AddTaskItemCommand(GetUserAggregateId(request.RequestUserId).Value, taskItemAggregateId, request.Title, request.Details, assignedUserAggregateId));

            int taskItemModelId = _identityLookup.GetModelId<Domain.TaskItem>(taskItemAggregateId);

            return new AddTaskItemResponse()
                       {
                           StatusCode = StatusOk,
                           TaskItemId = taskItemModelId,
                       };
        }

        public EditTaskItemResponse EditTaskItem(EditTaskItemRequest request)
        {
            Guid taskItemAggregateId = _identityLookup.GetAggregateId<Domain.TaskItem>(request.TaskItemId);

            Guid? assignedUserAggregateId = GetUserAggregateId(request.AssignedToUserId);

            _commandBus.Publish(new EditTaskItemCommand(GetUserAggregateId(request.RequestUserId).Value, taskItemAggregateId, request.Title, request.Details, assignedUserAggregateId));

            return new EditTaskItemResponse()
            {
                StatusCode = StatusOk,
                TaskItemId = request.TaskItemId,
            };
        }

        public AddCommentResponse AddComment(AddCommentRequest request)
        {
            Guid taskItemAggregateId = _identityLookup.GetAggregateId<Domain.TaskItem>(request.TaskItemId);
            Guid commentId = Guid.NewGuid();

            _commandBus.Publish(new AddCommentCommand(GetUserAggregateId(request.RequestUserId).Value, taskItemAggregateId, commentId, request.Comment));

            int commentModelId = _identityLookup.GetModelId<Domain.TaskComment>(commentId);


            return new AddCommentResponse()
                       {
                           StatusCode = StatusOk, 
                           CommentId = commentModelId
                       };
        }

        private Guid? GetUserAggregateId(int? userId)
        {
            return userId != null ? _identityLookup.GetAggregateId<Domain.User>(userId.Value) : (Guid?)null;
        }

        public FindTaskItemsByNameResponse FindTaskItemsByName(FindTaskItemsByNameRequest request)
        {
            var taskItems = _taskItemRepository.Find(ti => ti.Title.Contains(request.Query));
            
            return new FindTaskItemsByNameResponse() {StatusCode = StatusOk, TaskItems = Mapper.Map<IEnumerable<Domain.TaskItem>, List<TaskItem>>(taskItems)};
        }

        public FindTaskItemsByAssignedUserResponse FindTaskItemsByAssignedUser(FindTaskItemsByAssignedUserRequest request)
        {
            var taskItems = _taskItemRepository.Find(ti => ti.AssignedToUserId == request.AssignedToUserId);

            return new FindTaskItemsByAssignedUserResponse() { StatusCode = StatusOk, TaskItems = Mapper.Map<IEnumerable<Domain.TaskItem>, List<TaskItem>>(taskItems) };
        }
    }
}