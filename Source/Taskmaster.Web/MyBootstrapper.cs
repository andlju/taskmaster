using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ViewEngines;
using Petite;
using Taskmaster.DataAccess;
using Taskmaster.Domain;
using Taskmaster.Service;
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
                                                               c.Resolve<IObjectContext>(),
                                                               c.Resolve<ITaskItemRepository>(),
                                                               c.Resolve<IUserRepository>()
                                                           ));
            container.Register<IUserService>((c, p) => new UserService(
                                                                c.Resolve<IUserRepository>(),
                                                                c.Resolve<IObjectContext>()
                                                           ));
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