using System;
using System.Collections.ObjectModel;

namespace Taskmaster.Domain
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public int? AssignedToUserId { get; set; }
        public User AssignedTo { get; set; }
        
        public int CreatedByUserId { get; set; }
        public User CreatedBy { get; set; }

        public Collection<TaskComment> Comments { get; set; }
    }
}