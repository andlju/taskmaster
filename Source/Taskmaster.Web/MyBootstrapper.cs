using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ViewEngines;
using Petite;
using Taskmaster.DataAccess;
using Taskmaster.Domain;
using Taskmaster.Service;
using Taskmaster.Service.Bus;
using Taskmaster.Service.CommandHandlers;
using TinyIoC;

namespace Taskmaster.Web
{
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
                                                               c.Resolve<ITaskItemRepository>(),
                                                               c.Resolve<ICommandBus>(),
                                                               c.Resolve<IIdentityLookup>()
                                                           ));

            container.Register<IUserService>((c, p) => new UserService(
                                                           c.Resolve<IUserRepository>(),
                                                           c.Resolve<ICommandBus>(),
                                                           c.Resolve<IIdentityLookup>()
                                                           ));

            container.Register<IIdentityLookup>((c, p) => new IdentityLookup(new TaskmasterContext()));

            container.Register<ICommandBus>((c, p) =>
                                                {
                                                    // TODO Should find a better way than recreating the bus on every request for it...
                                                    var bus = new CommandBus();
                                                    var taskRepo = c.Resolve<ITaskItemRepository>();
                                                    var userRepo = c.Resolve<IUserRepository>();
                                                    var objContext = c.Resolve<IObjectContext>();
                                                    var idLookup = c.Resolve<IIdentityLookup>();

                                                    bus.RegisterHandler(new AddTaskItemCommandHandler(taskRepo, objContext, idLookup));
                                                    bus.RegisterHandler(new EditTaskItemCommandHandler(taskRepo, objContext, idLookup));
                                                    bus.RegisterHandler(new AddCommentCommandHandler(taskRepo, objContext, idLookup));
                                                    bus.RegisterHandler(new AddUserCommandHandler(userRepo, objContext, idLookup));

                                                    return bus;
                                                });
  
        }

        protected override IEnumerable<IStartup> GetStartupTasks()
        {
            yield return new DatabaseContextStartup();
        }
    }

    class DatabaseContextStartup : IStartup
    {

        public void Initialize()
        {
            new TaskmasterContext().Database.Initialize(force: false);
        }
    }
}