using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddUserCommandHandler : CommandHandlerBase, ICommandHandler<AddUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public AddUserCommandHandler(IUserRepository userRepository, IEventStorage storage)
            : base(storage)
        {
            _userRepository = userRepository;
        }

        public void Handle(AddUserCommand command)
        {
            // TODO Check for conflicting user name?

            Store(new UserAddedEvent(command.UserAggregateId, command.Name, command.AuthenticatedUserId));
        }
    }
}