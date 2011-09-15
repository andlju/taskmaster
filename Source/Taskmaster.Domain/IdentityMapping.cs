using System;
using System.ComponentModel.DataAnnotations;

namespace Taskmaster.Domain
{
    public class IdentityMapping
    {
        [Key]
        public Guid AggregateId { get; set; }

        public string AggregateType { get; set; }
        public int ModelId { get; set; }
    }
}