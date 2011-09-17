using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddCommentCommandHandler : CommandHandlerBase, ICommandHandler<AddCommentCommand>
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IObjectContext _objectContext;

        public AddCommentCommandHandler(ITaskItemRepository taskItemRepository, IObjectContext objectContext, IIdentityLookup identityLookup) : base(identityLookup)
        {
            _taskItemRepository = taskItemRepository;
            _objectContext = objectContext;
        }

        public void Handle(AddCommentCommand command)
        {
            var taskModelId = _identityLookup.GetModelId<Domain.TaskItem>(command.TaskItemAggregateId);

            var createdByModelid = GetUserModelId(command.AuthenticatedUserId).Value;

            var taskItem = _taskItemRepository.Get(t => t.TaskItemId == taskModelId);

            var comment = new Domain.TaskComment()
                              {
                                  Comment = command.Comment,
                                  CreatedByUserId = createdByModelid
                              };
            taskItem.Comments.Add(comment);

            _objectContext.SaveChanges();

            _identityLookup.StoreMapping<Domain.TaskComment>(command.CommentId, comment.TaskCommentId);
        }
    }
}