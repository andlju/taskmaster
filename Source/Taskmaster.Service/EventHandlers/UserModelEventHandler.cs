using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Events;

namespace Taskmaster.Service.EventHandlers
{
    public class UserModelHandler : IEventHandler<UserAddedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IObjectContext _objectContext;
        private readonly IIdentityLookup _identityLookup;

        public UserModelHandler(IUserRepository userRepository, IObjectContext objectContext, IIdentityLookup identityLookup)
        {
            _userRepository = userRepository;
            _objectContext = objectContext;
            _identityLookup = identityLookup;
        }

        public void Handle(UserAddedEvent evt)
        {
            var user = new Domain.User() { Name = evt.Name };
            _userRepository.Add(user);

            _objectContext.SaveChanges();

            _identityLookup.StoreMapping<Domain.User>(evt.UserAggregateId, user.UserId);
        }
    }
}