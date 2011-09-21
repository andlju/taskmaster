using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddTaskItemCommandHandler : CommandHandlerBase, ICommandHandler<AddTaskItemCommand>
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IObjectContext _objectContext;

        public AddTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IObjectContext objectContext, IIdentityLookup identityLookup, IEventStorage storage)
            : base(identityLookup, storage)
        {
            _taskItemRepository = taskItemRepository;
            _objectContext = objectContext;
        }

        public void Handle(AddTaskItemCommand command)
        {
            int createdByUserModelId = GetUserModelId(command.AuthenticatedUserId).GetValueOrDefault();
            int? assignedToUserModelId = GetUserModelId(command.AssignedUserAggregateId);

            var taskItem = new Domain.TaskItem()
                               {
                                   CreatedByUserId = createdByUserModelId,
                                   Title = command.Title,
                                   Details = command.Details,
                                   AssignedToUserId = assignedToUserModelId,
                               };
            _taskItemRepository.Add(taskItem);

            _objectContext.SaveChanges();

            IdentityLookup.StoreMapping<Domain.TaskItem>(command.TaskItemAggregateId, taskItem.TaskItemId);

            Store(new TaskItemAddedEvent(command.TaskItemAggregateId, command.Title, command.Details, command.AssignedUserAggregateId));
        }

    }
}