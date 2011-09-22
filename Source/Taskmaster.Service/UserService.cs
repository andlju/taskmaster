using System;
using System.Linq;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.Bus;
using Taskmaster.Service.Commands;

namespace Taskmaster.Service
{
    public class UserService : IUserService
    {
        public const int StatusOk = 0;
        public const int StatusNotFound = 1;

        private readonly IUserRepository _userRepository;
        private readonly ICommandBus _commandBus;
        private readonly IIdentityLookup _identityLookup;

        public UserService(IUserRepository userRepository, ICommandBus commandBus, IIdentityLookup identityLookup)
        {
            _userRepository = userRepository;
            _commandBus = commandBus;
            _identityLookup = identityLookup;
        }

        public AddUserResponse AddUser(AddUserRequest request)
        {
            var userAggregateId = Guid.NewGuid();

            _commandBus.Publish(new AddUserCommand(GetUserAggregateId(request.RequestUserId).Value, userAggregateId, request.Name));

            var userModelId = _identityLookup.GetModelId<Domain.User>(userAggregateId);

            return new AddUserResponse()
                       {
                           StatusCode = StatusOk,
                           UserId = userModelId
                       };
        }
        
        public ListUsersResponse ListUsers(ListUsersRequest request)
        {
            var users = _userRepository.List().Select(u => new User() {UserId = u.UserId, Name = u.Name});

            return new ListUsersResponse()
                       {
                           StatusCode = StatusOk,
                           Users = users
                       };
        }

        private Guid? GetUserAggregateId(int? userId)
        {
            return userId != null ? _identityLookup.GetAggregateId<Domain.User>(userId.Value) : (Guid?)null;
        }
    }
}