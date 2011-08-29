using System;
using System.Collections.Generic;

namespace Taskmaster.Service
{
    public class User
    {
        public int MemberId { get; set; }

        public string Name { get; set; }

    }

    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public DateTime DueDate { get; set; }

        public int AssignedMemberId { get; set; }
    }

    public class RequestBase
    {
        public int UserId { get; set; }
    }

    public class ResponseBase
    {
        public int StatusCode { get; set; }
    }

    public class AddTaskItemRequest : RequestBase
    {
        public string Title { get; set; }
        public string Details { get; set; }
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
        public int AssignedUserId { get; set; }
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