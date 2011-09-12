using System;
using System.Collections.Generic;

namespace Taskmaster.Service
{

    public class TaskComment
    {
        public string Comment { get; set; }
        public int CreatedByUserId { get; set; }
    }

    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public IEnumerable<TaskComment> Comments { get; set; }

        public int CreatedByUserId { get; set; }
        public int AssignedToUserId { get; set; }
    }

    public class RequestBase
    {
        public int RequestUserId { get; set; }
    }

    public class ResponseBase
    {
        public int StatusCode { get; set; }
    }

    public class AddTaskItemRequest : RequestBase
    {
        public string Title { get; set; }
        public string Details { get; set; }

        public int? AssignedToUserId { get; set; }
    }

    public class AddTaskItemResponse : ResponseBase
    {
        public int TaskItemId { get; set; }
    }

    public class EditTaskItemRequest : RequestBase
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public int? AssignedToUserId { get; set; }
    }

    public class EditTaskItemResponse : ResponseBase
    {
        public int TaskItemId { get; set; }
    }

    public class FindTaskItemsByNameRequest : RequestBase
    {
        public string Query { get; set; }
    }

    public class FindTaskItemsByNameResponse : ResponseBase
    {
        public List<TaskItem> TaskItems { get; set; }
    }

    public class FindTaskItemsByAssignedUserRequest : RequestBase
    {
        public int AssignedToUserId { get; set; }
    }

    public class FindTaskItemsByAssignedUserResponse : ResponseBase
    {
        public List<TaskItem> TaskItems { get; set; }
    }

    public interface ITaskService
    {
        AddTaskItemResponse AddTaskItem(AddTaskItemRequest request);
        EditTaskItemResponse EditTaskItem(EditTaskItemRequest request);

        FindTaskItemsByNameResponse FindTaskItemsByName(FindTaskItemsByNameRequest request);
        FindTaskItemsByAssignedUserResponse FindTaskItemsByAssignedUser(FindTaskItemsByAssignedUserRequest request);
    }
}