using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ViewEngines;
using Petite;
using Taskmaster.DataAccess;
using Taskmaster.Domain;
using Taskmaster.Service;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.Infrastructure;
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

            container.Register<IEventStorage>((c, p) => new EventStoreStorage(new EventBus()));
            
            container.Register<ICommandBus>((c, p) =>
                                                {
                                                    // TODO Should find a better way than recreating the buses on every request for it...
                                                    var bus = new CommandBus();

                                                    var taskRepo = c.Resolve<ITaskItemRepository>();
                                                    var userRepo = c.Resolve<IUserRepository>();
                                                    var objContext = c.Resolve<IObjectContext>();
                                                    var idLookup = c.Resolve<IIdentityLookup>();
                                                    var storage = c.Resolve<IEventStorage>();

                                                    bus.RegisterHandler(new AddTaskItemCommandHandler(taskRepo, objContext, idLookup, storage));
                                                    bus.RegisterHandler(new EditTaskItemCommandHandler(taskRepo, objContext, idLookup, storage));
                                                    bus.RegisterHandler(new AddCommentCommandHandler(taskRepo, objContext, idLookup, storage));
                                                    bus.RegisterHandler(new AddUserCommandHandler(userRepo, objContext, idLookup, storage));

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