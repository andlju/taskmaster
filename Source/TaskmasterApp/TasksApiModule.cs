using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Petite;
using Taskmaster.DataAccess;
using Taskmaster.Domain;
using Taskmaster.Service;
using TinyIoC;

namespace TaskmasterApp
{
    public class TaskViewModel
    {
        public int? TaskItemId { get; set; }

        public string Title { get; set; }
        public string Details { get; set; }
    }

    public class MyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register(new SingleUsageObjectContextAdapter(new DbContextFactory<TaskmasterContext>()));

            container.Register<IDbSetProvider>((c,p) => c.Resolve<SingleUsageObjectContextAdapter>());
            container.Register<IObjectContext>((c,p) => c.Resolve<SingleUsageObjectContextAdapter>());

            container.Register<ITaskItemRepository>((c, p) => new TaskItemRepository(c.Resolve<IDbSetProvider>()));
            container.Register<IUserRepository>((c, p) => new UserRepository(c.Resolve<IDbSetProvider>()));

            container.Register<ITaskService>((c, p) => new TaskService(
                                                           c.Resolve<IObjectContext>(),
                                                           c.Resolve<ITaskItemRepository>(),
                                                           c.Resolve<IUserRepository>()
                                                           ));
        }
    }

    public class TasksApiModule : NancyModule 
    {
        public TasksApiModule(ITaskService taskService) : base("/api")
        {
            Get["/tasks"] = parameters =>
                           {
                               return
                                   new JsonResponse(
                                       taskService.FindTaskItemsByName(new FindTaskItemsByNameRequest()
                                                                           {Query = "", UserId = 1}).TaskItems);
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
                                                                                   Details = taskViewModel.Details
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
                                                                                });
                                    return new JsonResponse(response.TaskItemId);
                                }

                            };
        }

    }
}