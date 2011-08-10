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
        public TaskItem TaskItem { get; set; }
    }

    public class EditTaskItemRequest : RequestBase
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }

    public class EditTaskItemResponse : ResponseBase
    {
        public TaskItem TaskItem { get; set; }
    }

    public class FindTaskItemByNameRequest : RequestBase
    {
        public string Query { get; set; }
    }

    public class FindTaskItemByNameResponse : ResponseBase
    {
        public List<TaskItem> TaskItems { get; set; }
    }

    public class FindTaskItemByAssignedUserRequest : RequestBase
    {
        public string AssignedUserId { get; set; }
    }

    public class FindTaskItemByAssignedUserResponse : ResponseBase
    {
        public List<TaskItem> TaskItems { get; set; }
    }

    public interface ITaskService
    {
        AddTaskItemResponse AddTaskItem(AddTaskItemRequest request);
        EditTaskItemResponse EditTaskItem(EditTaskItemRequest request);

        FindTaskItemByNameResponse FindTaskItemByName(FindTaskItemByNameRequest request);
        FindTaskItemByAssignedUserResponse FindTaskItemByAssignedUser(FindTaskItemByAssignedUserRequest request);
    }
}