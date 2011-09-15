using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddTaskItemCommandHandler : ICommandHandler<AddTaskItemCommand>
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IObjectContext _objectContext;
        private readonly IIdentityLookup _identityLookup;

        public AddTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IObjectContext objectContext, IIdentityLookup identityLookup)
        {
            _taskItemRepository = taskItemRepository;
            _objectContext = objectContext;
            _identityLookup = identityLookup;
        }

        public void Handle(AddTaskItemCommand command)
        {
            int? assignedToUserModelId = null;
            if (command.AssignedUserAggregateId != null)
            {
                assignedToUserModelId = _identityLookup.GetModelId<Domain.User>(command.AssignedUserAggregateId.Value);
            }

            var taskItem = new Domain.TaskItem()
                               {
                                   CreatedByUserId = 1,
                                   Title = command.Title,
                                   Details = command.Details,
                                   AssignedToUserId = assignedToUserModelId
                               };
            _taskItemRepository.Add(taskItem);

            _objectContext.SaveChanges();

            _identityLookup.StoreMapping<Domain.TaskItem>(command.TaskItemAggregateId, taskItem.TaskItemId);
        }
    }
}