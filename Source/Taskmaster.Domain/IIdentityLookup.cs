using System;

namespace Taskmaster.Domain
{
    public interface IIdentityLookup
    {
        Guid GetAggregateId<T>(int modelId);
        int GetModelId<T>(Guid aggregateId);

        void StoreMapping<T>(Guid aggregateId, int modelId);
        void RemoveMapping<T>(Guid aggregateId);
        void RemoveMapping<T>(int modelId);
    }
}