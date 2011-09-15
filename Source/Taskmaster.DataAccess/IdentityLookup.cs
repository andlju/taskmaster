using System;
using System.Linq;
using Taskmaster.Domain;

namespace Taskmaster.DataAccess
{
    public class IdentityLookup : IIdentityLookup
    {
        private readonly TaskmasterContext _context;

        public IdentityLookup(TaskmasterContext context)
        {
            _context = context;
        }

        public Guid GetAggregateId<T>(int modelId)
        {
            var mapping = GetMapping<T>(modelId);

            return mapping.AggregateId;
        }

        public int GetModelId<T>(Guid aggregateId)
        {
            var mapping = GetMapping<T>(aggregateId);

            return mapping.ModelId;
        }

        public void StoreMapping<T>(Guid aggregateId, int modelId)
        {
            var aggregateName = GetAggregateName(typeof(T));

            var mapping = new IdentityMapping()
                              {
                                  AggregateId = aggregateId,
                                  ModelId = modelId,
                                  AggregateType = aggregateName
                              };
            _context.IdentityMappings.Add(mapping);
            _context.SaveChanges();
        }

        public void RemoveMapping<T>(Guid aggregateId)
        {
            var mapping = GetMapping<T>(aggregateId);
            _context.IdentityMappings.Remove(mapping);
            _context.SaveChanges();
        }

        public void RemoveMapping<T>(int modelId)
        {
            var mapping = GetMapping<T>(modelId);
            _context.IdentityMappings.Remove(mapping);
            _context.SaveChanges();
        }

        private IdentityMapping GetMapping<T>(Guid aggregateId)
        {
            var aggregateName = GetAggregateName(typeof (T));
            var mapping = (from m in _context.IdentityMappings
                           where m.AggregateId == aggregateId &&
                                 m.AggregateType == aggregateName
                           select m).FirstOrDefault();
            if (mapping == null)
                throw new InvalidOperationException(string.Format("No '{0}' aggregate found with aggregate id {1}",
                                                                  aggregateName, aggregateId));
            return mapping;
        }

        private IdentityMapping GetMapping<T>(int modelId)
        {
            var aggregateName = GetAggregateName(typeof(T));
            var mapping = (from m in _context.IdentityMappings
                           where m.ModelId == modelId &&
                                 m.AggregateType == aggregateName
                           select m).FirstOrDefault();
            if (mapping == null)
                throw new InvalidOperationException(string.Format("No '{0}' aggregate found with model id {1}",
                                                                  aggregateName, modelId));
            return mapping;
        }

        private string GetAggregateName(Type aggregateType)
        {
            return aggregateType.Name;
        }

    }
}