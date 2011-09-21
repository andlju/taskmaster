using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Taskmaster.Service;

namespace Taskmaster.Web
{
    public class CommentViewModel
    {
        public int TaskItemId { get; set; }
        public int? CommentId { get; set; }
        public string Comment { get; set; }

        public int CreatedByUserId { get; set; }
        public int CreatedByUserName { get; set; }
    }

    public class TaskViewModel
    {
        public int? TaskItemId { get; set; }

        public string Title { get; set; }
        public string Details { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public int? AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
    }

    public class TasksApiModule : NancyModule 
    {
        public TasksApiModule(ITaskService taskService) : base("/api")
        {
            Get["/tasks"] = parameters =>
                                {
                                    var tasks =
                                        taskService.FindTaskItemsByName(new FindTaskItemsByNameRequest()
                                                                            {Query = "", RequestUserId = 1}).TaskItems;
                                    var response =
                                        new JsonResponse(tasks.Select(t =>
                                                                      new TaskViewModel()
                                                                          {
                                                                              TaskItemId = t.TaskItemId,
                                                                              Title = t.Title,
                                                                              Details = t.Details,
                                                                              AssignedToUserId =
                                                                                  t.AssignedToUserId > 0
                                                                                      ? (int?) t.AssignedToUserId
                                                                                      : null,
                                                                                      Comments = t.Comments.Select(c => new CommentViewModel()
                                                                                                                            {
                                                                                                                                TaskItemId = t.TaskItemId,
                                                                                                                                CommentId = c.CommentId,
                                                                                                                                CreatedByUserId = c.CreatedByUserId,
                                                                                                                                Comment = c.Comment
                                                                                                                            })
                                                                          })
                                            ).WithNoCache();

                                    return response;
                                };

            Post["/tasks"] = parameters =>
                            {
                                var taskViewModel = this.Bind<TaskViewModel>();

                                var taskId = taskViewModel.TaskItemId.GetValueOrDefault(0);
                                if (taskId == 0)
                                {
                                    var response = taskService.AddTaskItem(new AddTaskItemRequest()
                                                                               {
                                                                                   Title = taskViewModel.Title,
                                                                                   Details = taskViewModel.Details,
                                                                                   RequestUserId = int.Parse(Context.Request.Headers["TM-RequestUserId"].First()),
                                                                                   AssignedToUserId = taskViewModel.AssignedToUserId
                                                                               });
                                    return new JsonResponse(response.TaskItemId);
                                }
                                else
                                {
                                    var response = taskService.EditTaskItem(new EditTaskItemRequest()
                                                                                {
                                                                                    TaskItemId = taskId,
                                                                                    Title = taskViewModel.Title,
                                                                                    Details = taskViewModel.Details,
                                                                                    RequestUserId = int.Parse(Context.Request.Headers["TM-RequestUserId"].First()),
                                                                                    AssignedToUserId = taskViewModel.AssignedToUserId,
                                                                                });
                                    return new JsonResponse(response.TaskItemId);
                                }

                            };

            Post["/tasks/comments"] = parameters =>
                                         {
                                             var commentViewModel = this.Bind<CommentViewModel>();
                                             var commentId = commentViewModel.CommentId.GetValueOrDefault(0);
                                             if (commentId == 0)
                                                 return new JsonResponse("Unknown Task");
                                             
                                            var createdById =
                                                int.Parse(Context.Request.Headers["TM-RequestUserId"].First());
                                            var response = taskService.AddComment(new AddCommentRequest() { Comment = commentViewModel.Comment, TaskItemId = commentViewModel.TaskItemId, RequestUserId = createdById });

                                            return new JsonResponse(response.CommentId);
                                         };
        }

    }
}