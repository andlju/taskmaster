using System;
using Taskmaster.Domain;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.Service.CommandHandlers
{
    public class CommandHandlerBase
    {
        private readonly IIdentityLookup _identityLookup;
        private IEventStorage _storage;

        public CommandHandlerBase(IIdentityLookup identityLookup, IEventStorage storage)
        {
            _identityLookup = identityLookup;
            _storage = storage;
        }

        protected IIdentityLookup IdentityLookup
        {
            get { return _identityLookup; }
        }

        protected void Store<T>(T evt) where T : IEvent
        {
            _storage.Store(evt);
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