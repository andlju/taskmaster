namespace Taskmaster.Domain
{
    public class TaskComment
    {
        public int TaskCommentId { get; set; }
        public int TaskItemId { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }
        public int CreatedByUserId { get; set; }
    }
}