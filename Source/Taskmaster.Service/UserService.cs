using System.Linq;
using Petite;
using Taskmaster.Domain;

namespace Taskmaster.Service
{
    public class UserService : IUserService
    {
        public const int StatusOk = 0;
        public const int StatusNotFound = 1;

        private readonly IUserRepository _userRepository;
        private readonly IObjectContext _context;

        public UserService(IUserRepository userRepository, IObjectContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public AddUserResponse AddUser(AddUserRequest request)
        {
            var user = new Domain.User() { Name = request.Name };
            
            _userRepository.Add(user);

            _context.SaveChanges();

            return new AddUserResponse()
                       {
                           StatusCode = StatusOk,
                           UserId = user.UserId
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
    }
}