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
        private readonly IObjectContext _objectContext;

        public AddUserCommandHandler(IUserRepository userRepository, IObjectContext objectContext, IIdentityLookup identityLookup, IEventStorage storage)
            : base(identityLookup, storage)
        {
            _userRepository = userRepository;
            _objectContext = objectContext;
        }

        public void Handle(AddUserCommand command)
        {
            var user = new Domain.User() {Name = command.Name};
            _userRepository.Add(user);

            _objectContext.SaveChanges();

            IdentityLookup.StoreMapping<Domain.User>(command.UserAggregateId, user.UserId);

            Store(new UserAddedEvent(command.UserAggregateId, command.Name, command.AuthenticatedUserId));
        }
    }
}