using System;

namespace Taskmaster.Service.Commands
{
    public class AddUserCommand
    {
        public readonly Guid UserAggregateId;
        public readonly string Name;

        public AddUserCommand(Guid userAggregateId, string name)
        {
            UserAggregateId = userAggregateId;
            Name = name;
        }
    }
}