using System;

namespace Taskmaster.Service.Commands
{
    public class AddUserCommand : Command
    {
        public readonly Guid UserAggregateId;
        public readonly string Name;

        public AddUserCommand(Guid authenticatedUserId, Guid userAggregateId, string name) : base(authenticatedUserId)
        {
            UserAggregateId = userAggregateId;
            Name = name;
        }
    }
}