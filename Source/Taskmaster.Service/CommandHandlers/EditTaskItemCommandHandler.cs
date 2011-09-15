using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;

namespace Taskmaster.Service.CommandHandlers
{
    public class EditTaskItemCommandHandler : ICommandHandler<EditTaskItemCommand>
    {

        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IObjectContext _objectContext;
        private readonly IIdentityLookup _identityLookup;

        public EditTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IObjectContext objectContext, IIdentityLookup identityLookup)
        {
            _taskItemRepository = taskItemRepository;
            _objectContext = objectContext;
            _identityLookup = identityLookup;
        }

        public void Handle(EditTaskItemCommand command)
        {
            var taskItemIdModel = _identityLookup.GetModelId<Domain.TaskItem>(command.TaskItemAggregateId);

            int? assignedToUserModelId = null;
            if (command.AssignedUserAggregateId != null)
            {
                assignedToUserModelId = _identityLookup.GetModelId<Domain.User>(command.AssignedUserAggregateId.Value);
            }

            var taskItem = _taskItemRepository.Get(t => t.TaskItemId == taskItemIdModel);

            taskItem.Title = command.Title;
            taskItem.Details = command.Details;
            taskItem.AssignedToUserId = assignedToUserModelId;

            _objectContext.SaveChanges();
        }
    }
}