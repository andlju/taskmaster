using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddCommentCommandHandler : ICommandHandler<AddCommentCommand>
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IObjectContext _objectContext;
        private readonly IIdentityLookup _identityLookup;

        public AddCommentCommandHandler(ITaskItemRepository taskItemRepository, IObjectContext objectContext, IIdentityLookup identityLookup)
        {
            _taskItemRepository = taskItemRepository;
            _objectContext = objectContext;
            _identityLookup = identityLookup;
        }

        public void Handle(AddCommentCommand command)
        {
            var taskModelId = _identityLookup.GetModelId<Domain.TaskItem>(command.TaskItemAggregateId);

            var taskItem = _taskItemRepository.Get(t => t.TaskItemId == taskModelId);

            var comment = new Domain.TaskComment()
                              {
                                  Comment = command.Comment,
                              };
            taskItem.Comments.Add(comment);

            _objectContext.SaveChanges();

            _identityLookup.StoreMapping<Domain.TaskComment>(command.CommentId, comment.TaskCommentId);
        }
    }
}