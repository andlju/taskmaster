using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddCommentCommandHandler : CommandHandlerBase, ICommandHandler<AddCommentCommand>
    {
        public AddCommentCommandHandler(IEventStorage storage) :
            base(storage)
        {
        }

        public void Handle(AddCommentCommand command)
        {
            // TODO Check that task exists
            // Check user authorization

            Store(new CommentAddedEvent(command.TaskItemAggregateId, command.CommentId, command.Comment, command.AuthenticatedUserId));
        }
    }
}