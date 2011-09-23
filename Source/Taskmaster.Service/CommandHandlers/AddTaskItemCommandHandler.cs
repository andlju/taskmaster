using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddTaskItemCommandHandler : CommandHandlerBase, ICommandHandler<AddTaskItemCommand>
    {
        public AddTaskItemCommandHandler(IEventStorage storage)
            : base(storage)
        {
        }

        public void Handle(AddTaskItemCommand command)
        {
            // TODO Validate that Command is OK

            Store(new TaskItemAddedEvent(
                command.TaskItemAggregateId, 
                command.Title, 
                command.Details, 
                command.AssignedUserAggregateId, 
                command.AuthenticatedUserId));
        }

    }
}