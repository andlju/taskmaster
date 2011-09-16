using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Commands;

namespace Taskmaster.Service.CommandHandlers
{
    public class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        private IUserRepository _userRepository;
        private IObjectContext _objectContext;
        private IIdentityLookup _identityLookup;

        public AddUserCommandHandler(IUserRepository userRepository, IObjectContext objectContext, IIdentityLookup identityLookup)
        {
            _userRepository = userRepository;
            _objectContext = objectContext;
            _identityLookup = identityLookup;
        }

        public void Handle(AddUserCommand command)
        {
            var user = new Domain.User() {Name = command.Name};
            _userRepository.Add(user);

            _objectContext.SaveChanges();

            _identityLookup.StoreMapping<Domain.User>(command.UserAggregateId, user.UserId);
        }
    }
}