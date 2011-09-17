using System;
using Taskmaster.Domain;

namespace Taskmaster.Service.CommandHandlers
{
    public class CommandHandlerBase
    {
        protected readonly IIdentityLookup _identityLookup;

        public CommandHandlerBase(IIdentityLookup identityLookup)
        {
            _identityLookup = identityLookup;
        }

        protected int? GetUserModelId(Guid? assignedUserAggregateId)
        {
            int? assignedToUserModelId = null;
            if (assignedUserAggregateId != null)
            {
                assignedToUserModelId = _identityLookup.GetModelId<Domain.User>(assignedUserAggregateId.Value);
            }
            return assignedToUserModelId;
        }

    }
}