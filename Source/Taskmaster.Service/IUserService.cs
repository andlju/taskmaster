using System.Collections.Generic;

namespace Taskmaster.Service
{   
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; }
    }

    public class AddUserRequest : RequestBase
    {
        public string Name { get; set; }
    }

    public class AddUserResponse : ResponseBase
    {
        public int UserId { get; set; }
    }

    public class ListUsersRequest : RequestBase
    {
        
    }

    public class ListUsersResponse : ResponseBase
    {
        public IEnumerable<User> Users { get; set; }
    }

    public interface IUserService
    {
        AddUserResponse AddUser(AddUserRequest request);

        ListUsersResponse ListUsers(ListUsersRequest request);
    }
}