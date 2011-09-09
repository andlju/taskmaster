using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Taskmaster.Service;

namespace Taskmaster.Web
{
    public class UserViewModel
    {
        public int? UserId { get; set; }
        public string Name { get; set; }
    }

    public class UsersApiModule : NancyModule
    {
        public UsersApiModule(IUserService userService)
            : base("/api")
        {
            Get["/users"] = parameters =>
                                {
                                    var users = userService.ListUsers(new ListUsersRequest() {RequestUserId = 1}).Users;

                                    return new JsonResponse(users.Select(u => new User() {UserId = u.UserId, Name = u.Name})).
                                        WithNoCache();
                                };

            Post["/users"] = parameters =>
                                 {
                                     var user = this.Bind<UserViewModel>();
                                     var response = userService.AddUser(new AddUserRequest() {Name = user.Name, RequestUserId = 1});

                                     return new JsonResponse(response.UserId);
                                 };
        }
    }
}