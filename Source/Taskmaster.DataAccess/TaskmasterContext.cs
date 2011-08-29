using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using Petite;
using Taskmaster.Domain;

namespace Taskmaster.DataAccess
{
    public class TaskmasterContext : DbContext
    {
        public IDbSet<TaskItem> TaskItems { get; set; }

        public IDbSet<User> UserItems { get; set; }
    }

    public class TaskItemRepository : RepositoryBase<TaskItem>, ITaskItemRepository
    {
        public TaskItemRepository(IDbSetProvider objectSetProvider) : base(objectSetProvider)
        {
        }
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbSetProvider objectSetProvider)
            : base(objectSetProvider)
        {
            
        }
    }
}