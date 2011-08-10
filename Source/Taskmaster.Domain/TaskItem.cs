using Petite;

namespace Taskmaster.Domain
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public User AssignedTo { get; set; }
        public User CreatedBy { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }

    public interface ITaskItemRepository : IRepository<TaskItem>
    {
        
    }

    public interface IUserRepository : IRepository<User>
    {
        
    }
}