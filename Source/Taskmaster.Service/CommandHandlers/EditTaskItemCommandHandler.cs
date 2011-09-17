using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;

namespace Taskmaster.Service.CommandHandlers
{
    public class EditTaskItemCommandHandler : CommandHandlerBase, ICommandHandler<EditTaskItemCommand>
    {

        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IObjectContext _objectContext;

        public EditTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IObjectContext objectContext, IIdentityLookup identityLookup) : 
            base(identityLookup)
        {
            _taskItemRepository = taskItemRepository;
            _objectContext = objectContext;
        }

        public void Handle(EditTaskItemCommand command)
        {
            var taskItemIdModel = _identityLookup.GetModelId<Domain.TaskItem>(command.TaskItemAggregateId);

            int? assignedToUserModelId = GetUserModelId(command.AssignedUserAggregateId);

            var taskItem = _taskItemRepository.Get(t => t.TaskItemId == taskItemIdModel);

            taskItem.Title = command.Title;
            taskItem.Details = command.Details;
            taskItem.AssignedToUserId = assignedToUserModelId;

            _objectContext.SaveChanges();
        }
    }
}