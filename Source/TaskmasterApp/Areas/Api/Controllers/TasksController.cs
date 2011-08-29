using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Petite;
using Taskmaster.DataAccess;
using Taskmaster.Service;

namespace TaskmasterApp.Areas.Api.Controllers
{
    public class TaskViewModel
    {
        public int? TaskItemId { get; set; }

        public string Title { get; set; }
        public string Details { get; set; }
    }

    public class TasksController : Controller
    {
        private readonly ITaskService _service;
        private SingleUsageObjectContextAdapter _objectSetProvider;

        public TasksController()
        {
            _objectSetProvider = new Petite.SingleUsageObjectContextAdapter(new Petite.DbContextFactory<TaskmasterContext>());
            _service = new TaskService(_objectSetProvider, new TaskItemRepository(_objectSetProvider), new UserRepository(_objectSetProvider));
        }

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        //
        // GET: /Api/Tasks/

        public ActionResult Index()
        {
            var items = _service.FindTaskItemsByName(new FindTaskItemsByNameRequest() { Query = "", UserId = 1 });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(TaskViewModel taskViewModel)
        {
            var taskId = taskViewModel.TaskItemId.GetValueOrDefault(0);
            if (taskId == 0)
            {
                var response = _service.AddTaskItem(new AddTaskItemRequest()
                                         {
                                             Title = taskViewModel.Title,
                                             Details = taskViewModel.Details
                                         });
                return Json(response.TaskItemId);
            }
            else
            {
                var response = _service.EditTaskItem(new EditTaskItemRequest()
                                          {
                                              TaskItemId = taskId,
                                              Title = taskViewModel.Title,
                                              Details = taskViewModel.Details,
                                          });
                return Json(response.TaskItemId);
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _objectSetProvider.Dispose();
        }
    }
}
