using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service.CommandHandlers
{
    public class EditTaskItemCommandHandler : CommandHandlerBase, ICommandHandler<EditTaskItemCommand>
    {
        public EditTaskItemCommandHandler(IEventStorage storage) :
            base(storage)
        {
        }

        public void Handle(EditTaskItemCommand command)
        {
            // TODO Check user athorization

            Store(new TaskItemEditedEvent(command.TaskItemAggregateId, command.Title, command.Details, command.AssignedUserAggregateId, command.AuthenticatedUserId));
        }
    }
}