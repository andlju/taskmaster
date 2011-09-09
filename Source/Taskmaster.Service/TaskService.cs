using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Petite;

namespace Taskmaster.Service
{
    public class TaskService : ITaskService
    {
        public const int StatusOk = 0;
        public const int StatusNotFound = 1;

        private readonly IObjectContext _context;
        private readonly Domain.ITaskItemRepository _taskItemRepository;

        public TaskService(IObjectContext context, Domain.ITaskItemRepository taskItemRepository, Domain.IUserRepository userRepository)
        {
            _context = context;
            _taskItemRepository = taskItemRepository;

            Mapper.CreateMap<Domain.TaskItem, TaskItem>();
            Mapper.CreateMap<Domain.TaskComment, TaskComment>();
        }

        public AddTaskItemResponse AddTaskItem(AddTaskItemRequest request)
        {
            var taskItem = new Domain.TaskItem()
                               {
                                   Title = request.Title,
                                   Details = request.Details,
                                   CreatedByUserId = request.RequestUserId
                               };

            _taskItemRepository.Add(taskItem);
            _context.SaveChanges();

            return new AddTaskItemResponse()
                       {
                           StatusCode = StatusOk,
                           TaskItemId = taskItem.TaskItemId,
                       };
        }

        public EditTaskItemResponse EditTaskItem(EditTaskItemRequest request)
        {
            var taskItem = _taskItemRepository.Get(ti => ti.TaskItemId == request.TaskItemId);
            if (taskItem == null)
            {
                return new EditTaskItemResponse() {StatusCode = StatusNotFound};
            }
            taskItem.Title = request.Title;
            taskItem.Details = request.Details;
            taskItem.AssignedToUserId = request.AssignedToUserId;

            _context.SaveChanges();

            return new EditTaskItemResponse() {StatusCode = StatusOk, TaskItemId = taskItem.TaskItemId};
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