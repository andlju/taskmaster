using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddCommentCommandHandler : CommandHandlerBase, ICommandHandler<AddCommentCommand>
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IObjectContext _objectContext;

        public AddCommentCommandHandler(ITaskItemRepository taskItemRepository, IObjectContext objectContext, IIdentityLookup identityLookup, IEventStorage storage) :
            base(identityLookup, storage)
        {
            _taskItemRepository = taskItemRepository;
            _objectContext = objectContext;
        }

        public void Handle(AddCommentCommand command)
        {
            var taskModelId = IdentityLookup.GetModelId<Domain.TaskItem>(command.TaskItemAggregateId);

            var createdByModelid = GetUserModelId(command.AuthenticatedUserId).Value;

            var taskItem = _taskItemRepository.Get(t => t.TaskItemId == taskModelId);

            var comment = new Domain.TaskComment()
                              {
                                  Comment = command.Comment,
                                  CreatedByUserId = createdByModelid
                              };
            taskItem.Comments.Add(comment);

            _objectContext.SaveChanges();

            IdentityLookup.StoreMapping<Domain.TaskComment>(command.CommentId, comment.TaskCommentId);

            Store(new CommentAddedEvent(command.TaskItemAggregateId, command.CommentId, command.Comment, command.AuthenticatedUserId));
        }
    }
}